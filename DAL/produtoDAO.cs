using MercadoSeuZe.Modelo;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace MercadoSeuZe.DAL
{
    public class ProdutoDao
    {
        public void Inserir(Produto p)
        {
            using MySqlConnection conexao = Conexao.ObterConexao();
            string sql = @"INSERT INTO produto (nome,descricao,preco,estoque,categoria)
                           VALUES (@nome,@descricao,@preco,@estoque,@categoria)";
            using MySqlCommand cmd = new MySqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@nome", p.Nome);
            cmd.Parameters.AddWithValue("@descricao", p.Descricao);
            cmd.Parameters.AddWithValue("@preco", p.Preco);
            cmd.Parameters.AddWithValue("@estoque", p.Estoque);
            cmd.Parameters.AddWithValue("@categoria", p.Categoria);
            cmd.ExecuteNonQuery();
        }

        public void Atualizar(Produto p)
        {
            using MySqlConnection conexao = Conexao.ObterConexao();
            string sql = @"UPDATE produto SET nome=@nome,descricao=@descricao,
                           preco=@preco,estoque=@estoque,categoria=@categoria
                           WHERE id=@id";
            using MySqlCommand cmd = new MySqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@nome", p.Nome);
            cmd.Parameters.AddWithValue("@descricao", p.Descricao);
            cmd.Parameters.AddWithValue("@preco", p.Preco);
            cmd.Parameters.AddWithValue("@estoque", p.Estoque);
            cmd.Parameters.AddWithValue("@categoria", p.Categoria);
            cmd.Parameters.AddWithValue("@id", p.Id);
            cmd.ExecuteNonQuery();
        }

        public void Excluir(int id)
{
        using MySqlConnection conexao = Conexao.ObterConexao();

        string sqlItens =
        "DELETE FROM item_venda WHERE idProduto = @id";

        using MySqlCommand cmdItens =
        new MySqlCommand(sqlItens, conexao);
        cmdItens.Parameters.AddWithValue("@id", id);
        cmdItens.ExecuteNonQuery();

   
        using MySqlCommand cmd =
        new MySqlCommand("DELETE FROM produto WHERE id = @id", conexao);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }

        public void Mostrar(DataGridView grid)
        {
            using MySqlConnection conexao = Conexao.ObterConexao();
            using MySqlDataAdapter adapter = new MySqlDataAdapter(
                "SELECT id,nome,descricao,preco,estoque,categoria FROM produto", conexao);
            DataTable t = new DataTable();
            adapter.Fill(t);
            grid.DataSource = t;
        }

        public void Pesquisar(string texto, DataGridView grid)
        {
            using MySqlConnection conexao = Conexao.ObterConexao();
            string sql = @"SELECT id,nome,descricao,preco,estoque,categoria
                           FROM produto WHERE nome LIKE @nome";
            using MySqlCommand cmd = new MySqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@nome", "%" + texto + "%");
            using MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable t = new DataTable();
            adapter.Fill(t);
            grid.DataSource = t;
        }

        public List<Produto> Listar()
        {
            var list = new List<Produto>();
            using MySqlConnection conexao = Conexao.ObterConexao();
            using MySqlCommand cmd = new MySqlCommand(
                "SELECT id,nome,descricao,preco,estoque,categoria FROM produto WHERE estoque > 0", conexao);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new Produto
                {
                    Id = r.GetInt32("id"),
                    Nome = r.GetString("nome"),
                    Descricao = r.IsDBNull(r.GetOrdinal("descricao")) ? "" : r.GetString("descricao"),
                    Preco = r.GetDecimal("preco"),
                    Estoque = r.GetInt32("estoque"),
                    Categoria = r.IsDBNull(r.GetOrdinal("categoria")) ? "" : r.GetString("categoria")
                });
            }
            return list;
        }

        public Produto? BuscarPorId(int id)
        {
            using MySqlConnection conexao = Conexao.ObterConexao();
            using MySqlCommand cmd = new MySqlCommand(
                "SELECT id,nome,descricao,preco,estoque,categoria FROM produto WHERE id=@id", conexao);
            cmd.Parameters.AddWithValue("@id", id);
            using var r = cmd.ExecuteReader();
            if (r.Read())
            {
                return new Produto
                {
                    Id = r.GetInt32("id"),
                    Nome = r.GetString("nome"),
                    Descricao = r.IsDBNull(r.GetOrdinal("descricao")) ? "" : r.GetString("descricao"),
                    Preco = r.GetDecimal("preco"),
                    Estoque = r.GetInt32("estoque"),
                    Categoria = r.IsDBNull(r.GetOrdinal("categoria")) ? "" : r.GetString("categoria")
                };
            }
            return null;
        }
    }
}
