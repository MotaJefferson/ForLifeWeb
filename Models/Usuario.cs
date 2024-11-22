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
        [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF inválido")]
        public string cpf { get; set; }

        [Required]
        [StringLength(50)]
        public string cod_usuario { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string senha { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime data_cadastro { get; set; }


        private static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();
            return configuration.GetConnectionString("DefaultConnection");
        }

        public static bool ValidarLogin(string cod_usario, string senha)
        {
            var ret = false;
            var connectionString = GetConnectionString();

            using (var conexao = new SqlConnection(connectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;

                    comando.CommandText = string.Format(
                        "SELECT COUNT(*) FROM Usuarios WHERE cod_usuario = '{0}' AND senha = '{0}'", cod_usario, senha);

                    ret = ((int)comando.ExecuteScalar() > 0);
                }
            }
            
            return ret;

        }

    }
}
