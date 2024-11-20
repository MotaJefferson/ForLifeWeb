using System.ComponentModel.DataAnnotations;

namespace ForLifeWeb.Models
{
    public class ProdutoEstoque
    {
        [Key]
        public int id_estoque { get; set; }

        [Required]
        public int produto_id { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int quantidade_atual { get; set; }

        [Range(0, int.MaxValue)]
        public int quantidade_saida { get; set; }

        [Range(0, int.MaxValue)]
        public int quantidade_colheita { get; set; }

        [DataType(DataType.Date)]
        public DateTime data_colheita { get; set; }

        [DataType(DataType.Date)]
        public DateTime data_saida { get; set; }

        [DataType(DataType.Date)]
        public DateTime data_vencimento_estimado { get; set; }

        [DataType(DataType.Date)]
        public DateTime data_registro { get; set; }
    }
}
