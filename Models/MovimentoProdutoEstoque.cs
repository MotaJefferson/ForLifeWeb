using Microsoft.EntityFrameworkCore;

namespace ForLifeWeb.Models
{
    [Keyless]
    public class MovimentoProdutoEstoque
    {
        public int idMovimentoProduto { get; set; }
        public string nome { get; set; }
        public string descrMovimento { get; set; }
        public int? quantidade_anterior { get; set; }
        public int quantidadeMovimento { get; set; }
        public int? quantidade_atual { get; set; }
        public DateTime? data_registro { get; set; }
    }
}
