using MercadoSeuZe.Modelo;

namespace MercadoSeuZe.BLL
{
    public class ProdutoBLL
    {
        public string? Validar(Produto p)
        {
            if (string.IsNullOrWhiteSpace(p.Nome)) return "Informe o nome do produto.";
            if (p.Preco <= 0) return "Preço deve ser maior que zero.";
            if (p.Estoque < 0) return "Estoque não pode ser negativo.";
            return null;
        }
    }

    public class VendaBLL
    {
        public string? Validar(Venda v)
        {
            if (v.IdCliente <= 0) return "Selecione um cliente.";
            if (v.Itens.Count == 0) return "Adicione pelo menos um item.";
            foreach (var i in v.Itens)
            {
                if (i.Quantidade <= 0) return "Quantidade inválida.";
            }
            return null;
        }
    }
}
