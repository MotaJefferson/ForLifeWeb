using System.ComponentModel.DataAnnotations;

namespace ForLifeWeb.Models
{
    public class Fornecedor
    {
        [Key]
        public int id_fornecedor { get; set; }

        [StringLength(5)]
        public string? tipo { get; set; }

        [StringLength(100)]
        public string? nome { get; set; }

        [StringLength(100)]
        public string? razao_social { get; set; }

        [StringLength(15)]
        public string? cpf { get; set; }

        [StringLength(20)]
        public string? cnpj { get; set; }

        [StringLength(15)]
        public string? telefone { get; set; }

        [StringLength(500)]
        public string? observacoes { get; set; }

        [Required]
        public bool ativo { get; set; }
    }
}
