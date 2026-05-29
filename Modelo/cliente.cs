namespace MercadoSeuZe.Modelo
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string Cpf { get; set; } = "";
        public string Telefone { get; set; } = "";
        public string Email { get; set; } = "";
        public string EstadoCivil { get; set; } = "";
        public string Sexo { get; set; } = "";
        public string Endereco { get; set; } = "";
    }
}
