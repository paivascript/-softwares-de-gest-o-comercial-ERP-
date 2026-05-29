using MercadoSeuZe.Modelo;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;

namespace MercadoSeuZe.DAL
{
    public class ClienteDao
    {
        public void Inserir(Cliente cliente)
        {
            MySqlConnection conexao =
            Conexao.ObterConexao();

            string sql =
            @"INSERT INTO cliente
            (nome,cpf,telefone,email,estadoCivil,sexo,endereco)
            VALUES
            (@nome,@cpf,@telefone,@email,@estadoCivil,@sexo,@endereco)";

            MySqlCommand comando =
            new MySqlCommand(sql, conexao);

            comando.Parameters.AddWithValue(
            "@nome",
            cliente.Nome);

            comando.Parameters.AddWithValue(
            "@cpf",
            cliente.Cpf);

            comando.Parameters.AddWithValue(
            "@telefone",
            cliente.Telefone);

            comando.Parameters.AddWithValue(
            "@email",
            cliente.Email);

            comando.Parameters.AddWithValue(
            "@estadoCivil",
            cliente.EstadoCivil);

            comando.Parameters.AddWithValue(
            "@sexo",
            cliente.Sexo);

            comando.Parameters.AddWithValue(
            "@endereco",
            cliente.Endereco);

            comando.ExecuteNonQuery();

            conexao.Close();
        }

        public void Mostrar(DataGridView grid)
        {
            MySqlConnection conexao =
            Conexao.ObterConexao();

            string sql =
            "SELECT * FROM cliente";

            MySqlDataAdapter adapter =
            new MySqlDataAdapter(sql, conexao);

            DataTable tabela =
            new DataTable();

            adapter.Fill(tabela);

            grid.DataSource = tabela;

            conexao.Close();
        }

        public void Pesquisar(
        string texto,
        DataGridView grid)
        {
            MySqlConnection conexao =
            Conexao.ObterConexao();

            string sql =
            @"SELECT *
            FROM cliente
            WHERE nome LIKE @nome";

            MySqlCommand comando =
            new MySqlCommand(sql, conexao);

            comando.Parameters.AddWithValue(
            "@nome",
            "%" + texto + "%");

            MySqlDataAdapter adapter =
            new MySqlDataAdapter(comando);

            DataTable tabela =
            new DataTable();

            adapter.Fill(tabela);

            grid.DataSource = tabela;

            conexao.Close();
        }

        public System.Collections.Generic.List<Cliente> Listar()
        {
            var list = new System.Collections.Generic.List<Cliente>();
            using MySqlConnection conexao = Conexao.ObterConexao();
            using MySqlCommand cmd = new MySqlCommand(
                "SELECT id, nome FROM cliente ORDER BY nome", conexao);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new Cliente
                {
                    Id = r.GetInt32("id"),
                    Nome = r.GetString("nome")
                });
            }
            return list;
        }

        public void Excluir(int id)
{
    MySqlConnection conexao =
    Conexao.ObterConexao();

    // 1. Apaga os itens das vendas deste cliente
    string sqlItens =
    @"DELETE FROM item_venda 
      WHERE idVenda IN 
      (SELECT id FROM venda WHERE idCliente = @id)";

    MySqlCommand cmdItens =
    new MySqlCommand(sqlItens, conexao);
    cmdItens.Parameters.AddWithValue("@id", id);
    cmdItens.ExecuteNonQuery();

    // 2. Apaga as vendas do cliente
    string sqlVenda =
    "DELETE FROM venda WHERE idCliente = @id";

    MySqlCommand cmdVenda =
    new MySqlCommand(sqlVenda, conexao);
    cmdVenda.Parameters.AddWithValue("@id", id);
    cmdVenda.ExecuteNonQuery();

    // 3. Agora apaga o cliente
    string sqlCliente =
    "DELETE FROM cliente WHERE id = @id";

    MySqlCommand cmdCliente =
    new MySqlCommand(sqlCliente, conexao);
    cmdCliente.Parameters.AddWithValue("@id", id);
    cmdCliente.ExecuteNonQuery();

    conexao.Close();
}
    }
}