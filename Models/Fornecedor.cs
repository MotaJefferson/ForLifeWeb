using System.ComponentModel.DataAnnotations;

namespace ForLifeWeb.Models
{
    public class Fornecedor
    {
        [Key]
        public int id_fornecedor { get; set; }

        [Required]
        [StringLength(100)]
        public string nome { get; set; }

        [StringLength(15)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF inválido")]
        public string? cpf { get; set; }

        [StringLength(100)]
        public string? razao_social { get; set; }

        [RegularExpression(@"^\d{14}$", ErrorMessage = "CNPJ inválido")]
        [StringLength(20)]
        public string? cnpj { get; set; }

        [StringLength(15)]
        public string? telefone { get; set; }

        [StringLength(500)]
        public string? endereco { get; set; }
    }
}
