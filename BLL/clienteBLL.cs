using MercadoSeuZe.Modelo;

namespace MercadoSeuZe.BLL
{
    public class ClienteBLL
    {
        public bool Valida(Cliente cliente)
        {
            if (cliente.Nome == "")
                return false;

            if (cliente.Cpf == "")
                return false;

            if (cliente.Telefone == "")
                return false;

            return true;
        }
    }
}