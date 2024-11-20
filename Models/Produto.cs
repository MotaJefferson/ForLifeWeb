using System.ComponentModel.DataAnnotations;

namespace ForLifeWeb.Models
{
    public class Produto
    {
        [Key]
        public int id_produto { get; set; }

        [Required]
        public int insumo_id { get; set; }

        [Required]
        [StringLength(100)]
        public string nome { get; set; }

        [StringLength(500)]
        public string descricao { get; set; }

        [Range(1, 365, ErrorMessage = "Período de vencimento deve ser entre 1 e 365 dias.")]
        public int periodo_vencimento { get; set; }

        [Range(1, 365, ErrorMessage = "Período de colheita deve ser entre 1 e 365 dias.")]
        public int periodo_colheita { get; set; }

        [Range(1, 365, ErrorMessage = "Período limite de colheita deve ser entre 1 e 365 dias.")]
        public int periodo_limite_colheita { get; set; }
    }
}
