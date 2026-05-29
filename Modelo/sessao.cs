namespace MercadoSeuZe.Modelo
{
    /// <summary>
    /// Sessão do usuário logado, usada para vincular vendas e logs.
    /// </summary>
    public static class SessaoUsuario
    {
        public static int IdUsuario { get; set; }
        public static string Login { get; set; } = "";
    }
}
