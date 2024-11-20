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

        [Required]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF inválido")]
        public string cpf { get; set; }

        [StringLength(100)]
        public string razao_social { get; set; }

        [RegularExpression(@"^\d{14}$", ErrorMessage = "CNPJ inválido")]
        public string cnpj { get; set; }
    }
}
