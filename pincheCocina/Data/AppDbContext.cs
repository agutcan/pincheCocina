using Microsoft.EntityFrameworkCore;
using pincheCocina.MVVM.Models;

namespace pincheCocina.Data
{
    // Heredar de DbContext soluciona el error que tenías en MauiProgram
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // Crea la base de datos automáticamente si no existe
            Database.EnsureCreated();
        }

        // Estas son las tablas que se crearán en SQLite
        public DbSet<Receta> Recetas { get; set; }
        public DbSet<PasoReceta> PasosRecetas { get; set; }
        public DbSet<Ingrediente> Ingredientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuramos relaciones (opcional, EF suele detectarlas)
            modelBuilder.Entity<Receta>()
                .HasMany(r => r.Pasos)
                .WithOne()
                .HasForeignKey(p => p.RecetaId);

            modelBuilder.Entity<PasoReceta>()
                .HasMany(p => p.Ingredientes)
                .WithOne()
                .HasForeignKey(i => i.PasoRecetaId);
        }
    }
}