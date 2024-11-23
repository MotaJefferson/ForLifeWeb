using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ForLifeWeb.Models
{
    public class InsumoCompra
    {
        [Key]
        public int id_compra { get; set; }

        [Required]
        public int fornecedor_id { get; set; }

        [Required]
        public int insumo_id { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Preço unitário deve ser maior que zero.")]
        public decimal preco_unitario { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que zero.")]
        public int quantidade { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Valor de compra deve ser maior que zero.")]
        public decimal? valor_compra { get; set; }
    }
}
