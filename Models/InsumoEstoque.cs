using System.ComponentModel.DataAnnotations;

namespace ForLifeWeb.Models
{
    public class InsumoEstoque
    {
        [Key]
        public int id_estoque { get; set; }

        [Required]
        public int fornecedor_id { get; set; }

        [Required]
        public int insumo_id { get; set; }

        [Range(0, int.MaxValue)]
        public int? quantidade_anterior { get; set; }

        [Range(0, int.MaxValue)]
        public int? quantidade_atual { get; set; }

        [Range(0, int.MaxValue)]
        public int? quantidade_entrada { get; set; }

        [Range(0, int.MaxValue)]
        public int? quantidade_saida { get; set; }

        [DataType(DataType.Date)]
        public DateTime? data_entrada { get; set; }

        [DataType(DataType.Date)]
        public DateTime? data_saida { get; set; }

        [DataType(DataType.Date)]
        public DateTime? data_baixa { get; set; }

        [DataType(DataType.Date)]
        public DateTime? data_registro { get; set; } = DateTime.Now;

        [Required]
        [StringLength(1)] 
        public char tipo_movimento { get; set; } 
    }
}
