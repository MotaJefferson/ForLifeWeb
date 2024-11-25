using System.ComponentModel.DataAnnotations;

namespace ForLifeWeb.Models
{
    public class Plantio
    {
        [Key]
        public int id_plantio { get; set; }

        [Required]
        public int insumo_id { get; set; }

        [Required]
        public int produto_id { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantidade plantada deve ser maior que zero.")]
        public int quantidade_plantio { get; set; }

        [DataType(DataType.Date)]
        public DateTime? data_plantio { get; set; }

        [DataType(DataType.Date)]
        public DateTime? data_colheita { get; set; }

        [DataType(DataType.Date)]
        public DateTime? data_vencimento { get; set; }

        [DataType(DataType.Date)]
        public DateTime? data_registro { get; set; }

        [DataType(DataType.Date)]
        public DateTime? data_baixa { get; set; }
    }
}
