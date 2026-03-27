using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ItemInventario> Itens { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Local> Locais { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Aplicar configurações de Fluent API (opcional, mas recomendado para Clean Code)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
        // Exemplo: Garantir que nomes de categorias sejam únicos
        modelBuilder.Entity<Categoria>()
            .HasIndex(c => c.Nome)
            .IsUnique();
    }
}