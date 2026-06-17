using MercadoSeuZe.Modelo;
using MySqlConnector;    

namespace MercadoSeuZe.DAL
{
    public class UsuarioDao
    {
        public bool Login(Usuario usuario)
        {
            using MySqlConnection conexao = Conexao.ObterConexao();
            string sql = @"SELECT id, login FROM usuario
                           WHERE login=@login AND senha=@senha";
            using MySqlCommand comando = new MySqlCommand(sql, conexao);
            comando.Parameters.AddWithValue("@login", usuario.Login);
            comando.Parameters.AddWithValue("@senha", usuario.Senha);

            using MySqlDataReader reader = comando.ExecuteReader();
            if (reader.Read())
            {
                SessaoUsuario.IdUsuario = reader.GetInt32("id");
                SessaoUsuario.Login = reader.GetString("login");
                return true;
            }
            return false;
        }
    }
}
