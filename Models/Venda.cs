using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ForLifeWeb.Models
{
    public class Venda
    {
        [Key]
        public int id_venda { get; set; }

        [Required]
        public int produto_id { get; set; }

        [Required]
        public int cliente_id { get; set; }

        [StringLength(50)]
        public string? numero_venda { get; set; }

        [DataType(DataType.Date)]
        public DateTime? data_registro { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que zero.")]
        public int quantidade_venda { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime data_venda { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor da venda deve ser maior que zero.")]
        public decimal preco_unitario { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor da venda deve ser maior que zero.")]
        public decimal valor_venda { get; set; }

        [StringLength(10)]
        public string? forma_pagamento { get; set; }

    }
}
