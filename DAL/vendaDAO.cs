using MercadoSeuZe.Modelo;
using MySqlConnector;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace MercadoSeuZe.DAL
{
    public class VendaDao
    {
        public int Registrar(Venda venda)
        {
            using MySqlConnection conexao = Conexao.ObterConexao();
            using MySqlTransaction tx = conexao.BeginTransaction();
            try
            {
                using (var cmd = new MySqlCommand(
                    @"INSERT INTO venda (idCliente,idUsuario,valorTotal)
                      VALUES (@c,@u,@t); SELECT LAST_INSERT_ID();", conexao, tx))
                {
                    cmd.Parameters.AddWithValue("@c", venda.IdCliente);
                    cmd.Parameters.AddWithValue("@u", venda.IdUsuario);
                    cmd.Parameters.AddWithValue("@t", venda.ValorTotal);
                    venda.Id = System.Convert.ToInt32(cmd.ExecuteScalar());
                }

                foreach (var item in venda.Itens)
                {
                    using (var cmdItem = new MySqlCommand(
                        @"INSERT INTO item_venda
                          (idVenda,idProduto,quantidade,precoUnitario,subtotal)
                          VALUES (@v,@p,@q,@pu,@s)", conexao, tx))
                    {
                        cmdItem.Parameters.AddWithValue("@v", venda.Id);
                        cmdItem.Parameters.AddWithValue("@p", item.IdProduto);
                        cmdItem.Parameters.AddWithValue("@q", item.Quantidade);
                        cmdItem.Parameters.AddWithValue("@pu", item.PrecoUnitario);
                        cmdItem.Parameters.AddWithValue("@s", item.Subtotal);
                        cmdItem.ExecuteNonQuery();
                    }

                    using (var cmdEstoque = new MySqlCommand(
                        @"UPDATE produto SET estoque = estoque - @q
                          WHERE id=@p AND estoque >= @q", conexao, tx))
                    {
                        cmdEstoque.Parameters.AddWithValue("@q", item.Quantidade);
                        cmdEstoque.Parameters.AddWithValue("@p", item.IdProduto);
                        int afetadas = cmdEstoque.ExecuteNonQuery();
                        if (afetadas == 0)
                            throw new System.Exception(
                                $"Estoque insuficiente para o produto {item.NomeProduto}.");
                    }
                }

                tx.Commit();
                return venda.Id;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public void Mostrar(DataGridView grid)
        {
            using MySqlConnection conexao = Conexao.ObterConexao();
            string sql = @"SELECT v.id, c.nome AS cliente, u.login AS usuario,
                                  v.valorTotal, v.dataVenda
                           FROM venda v
                           LEFT JOIN cliente c ON c.id = v.idCliente
                           LEFT JOIN usuario u ON u.id = v.idUsuario
                           ORDER BY v.id DESC";
            using MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conexao);
            DataTable t = new DataTable();
            adapter.Fill(t);
            grid.DataSource = t;
        }

        public void Pesquisar(string termo, DataGridView grid)
        {
            using MySqlConnection conexao = Conexao.ObterConexao();
            string sql = @"SELECT v.id, c.nome AS cliente, u.login AS usuario,
                                  v.valorTotal, v.dataVenda
                           FROM venda v
                           LEFT JOIN cliente c ON c.id = v.idCliente
                           LEFT JOIN usuario u ON u.id = v.idUsuario
                           WHERE c.nome LIKE @t OR CAST(v.id AS CHAR) LIKE @t
                           ORDER BY v.id DESC";
            using var cmd = new MySqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@t", "%" + termo + "%");
            using MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable t = new DataTable();
            adapter.Fill(t);
            grid.DataSource = t;
        }

        public DataTable ListarItens(int idVenda)
        {
            using MySqlConnection conexao = Conexao.ObterConexao();
            string sql = @"SELECT p.nome AS produto,
                                  iv.quantidade AS qtd,
                                  iv.precoUnitario AS preco_unit,
                                  iv.subtotal
                           FROM item_venda iv
                           LEFT JOIN produto p ON p.id = iv.idProduto
                           WHERE iv.idVenda = @id";
            using var cmd = new MySqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@id", idVenda);
            using MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable t = new DataTable();
            adapter.Fill(t);
            return t;
        }
    }
}
