using System.Collections.Generic;

namespace MercadoSeuZe.Modelo
{
    public class Venda
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public decimal ValorTotal { get; set; }
        public List<ItemVenda> Itens { get; set; } = new List<ItemVenda>();
    }

    public class ItemVenda
    {
        public int Id { get; set; }
        public int IdVenda { get; set; }
        public int IdProduto { get; set; }
        public string NomeProduto { get; set; } = "";
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Subtotal => Quantidade * PrecoUnitario;
    }
}
