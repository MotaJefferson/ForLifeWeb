using System.ComponentModel.DataAnnotations;

namespace ForLifeWeb.Models
{
    public class Insumo
    {
        [Key]
        public int id_insumo { get; set; }

        [Required]
        [StringLength(100)]
        public string nome { get; set; }

        [StringLength(500)]
        public string descricao { get; set; }

        [Required]
        [StringLength(50)]
        public string tipo { get; set; }

        [Required]
        public bool ativo { get; set; }

        [Range(1, 365, ErrorMessage = "Período de vencimento deve ser entre 1 e 365 dias.")]
        public int periodo_vencimento { get; set; }
    }
}
