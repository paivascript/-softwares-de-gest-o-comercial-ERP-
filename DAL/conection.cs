using MySql.Data.MySqlClient;

namespace MercadoSeuZe.DAL
{
    public class Conexao
    {
        public static MySqlConnection ObterConexao()
        {
            MySqlConnection conexao =
            new MySqlConnection(
            "server=localhost;database=mercadoseuze;uid=root;pwd=Root@12345"
            );

            conexao.Open();

            return conexao;
        }
    }
}