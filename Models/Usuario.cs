using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.ComponentModel.DataAnnotations;

namespace ForLifeWeb.Models
{
    public class Usuario
    {
        [Key]
        public int id_usuario { get; set; }

        [Required]
        [StringLength(100)]
        public string nome { get; set; }

        [StringLength(50)]
        public string cargo { get; set; }

        [Required]
        [StringLength(15)]
        public string cpf { get; set; }

        [Required]
        [StringLength(50)]
        public string cod_usuario { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string senha { get; set; }

        [DataType(DataType.Date)]
        public DateTime? data_cadastro { get; set; }

        [Required]
        public bool ativo { get; set; }

    }
}
