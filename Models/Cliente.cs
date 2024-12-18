﻿using System.ComponentModel.DataAnnotations;

namespace ForLifeWeb.Models
{
    public class Cliente
    {
        [Key]
        public int id_cliente { get; set; }

        [Required]
        [StringLength(100)]
        public string nome { get; set; }

        [StringLength(15)]
        public string? telefone { get; set; }

        [Required]
        [StringLength(15)]
        public string cpf { get; set; }

        [StringLength(500)]
        public string? observacoes { get; set; }

        [Required]
        public bool ativo { get; set; }
    }
}
