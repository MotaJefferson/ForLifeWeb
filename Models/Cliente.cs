using System.ComponentModel.DataAnnotations;

namespace ForLifeWeb.Models
{
    public class Cliente
    {
        [Key]
        public int id_cliente { get; set; }

        [Required]
        [StringLength(100)]
        public string nome { get; set; }

        [Required]
        [StringLength(15)]
        public string telefone { get; set; }

        [Required]
        [StringLength(15)]
        //[RegularExpression(@"^\d{11}$", ErrorMessage = "CPF inválido")]
        public string cpf { get; set; }

        [StringLength(500)]
        public string endereco { get; set; }
    }
}
