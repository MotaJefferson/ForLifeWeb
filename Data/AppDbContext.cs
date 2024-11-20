using ForLifeWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ForLifeWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Insumo> Insumos { get; set; }
        public DbSet<InsumoCompra> InsumoCompra { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<Plantio> Plantio { get; set; }
        public DbSet<ProdutoEstoque> ProdutoEstoque { get; set; }
        public DbSet<InsumoEstoque> InsumoEstoque { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Chaves Primárias
            modelBuilder.Entity<Usuario>().HasKey(u => u.id_usuario);
            modelBuilder.Entity<Fornecedor>().HasKey(f => f.id_fornecedor);
            modelBuilder.Entity<Insumo>().HasKey(i => i.id_insumo);
            modelBuilder.Entity<InsumoCompra>().HasKey(ic => ic.id_compra);
            modelBuilder.Entity<Produto>().HasKey(p => p.id_produto);
            modelBuilder.Entity<Cliente>().HasKey(c => c.id_cliente);
            modelBuilder.Entity<Venda>().HasKey(v => v.id_venda);
            modelBuilder.Entity<Plantio>().HasKey(pl => pl.id_plantio);
            modelBuilder.Entity<ProdutoEstoque>().HasKey(pe => pe.id_estoque);
            modelBuilder.Entity<InsumoEstoque>().HasKey(ie => ie.id_estoque);

            // Configurações de Precisão
            modelBuilder.Entity<InsumoCompra>().Property(ic => ic.preco_unitario).HasPrecision(18, 4);
            modelBuilder.Entity<InsumoCompra>().Property(ic => ic.valor_compra).HasPrecision(18, 4);
            modelBuilder.Entity<Venda>().Property(v => v.valor_venda).HasPrecision(18, 4);

            // Relacionamentos e DeleteBehavior
            modelBuilder.Entity<InsumoCompra>()
                .HasOne<Fornecedor>()
                .WithMany()
                .HasForeignKey(ic => ic.fornecedor_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InsumoCompra>()
                .HasOne<Insumo>()
                .WithMany()
                .HasForeignKey(ic => ic.insumo_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Produto>()
                .HasOne<Insumo>()
                .WithMany()
                .HasForeignKey(p => p.insumo_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Venda>()
                .HasOne<Cliente>()
                .WithMany()
                .HasForeignKey(v => v.cliente_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Venda>()
                .HasOne<Produto>()
                .WithMany()
                .HasForeignKey(v => v.produto_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Plantio>()
                .HasOne<Produto>()
                .WithMany()
                .HasForeignKey(pl => pl.produto_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Plantio>()
                .HasOne<Insumo>()
                .WithMany()
                .HasForeignKey(pl => pl.insumo_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProdutoEstoque>()
                .HasOne<Produto>()
                .WithMany()
                .HasForeignKey(pe => pe.produto_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InsumoEstoque>()
                .HasOne<Insumo>()
                .WithMany()
                .HasForeignKey(ie => ie.insumo_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InsumoEstoque>()
                .HasOne<Fornecedor>()
                .WithMany()
                .HasForeignKey(ie => ie.fornecedor_id)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }

}
