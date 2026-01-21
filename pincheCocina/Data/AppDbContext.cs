using Microsoft.EntityFrameworkCore;
using pincheCocina.MVVM.Models;

namespace pincheCocina.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // 1. Borra la base de datos vieja (con esto eliminamos el rastro de los datos viejos)
            Database.EnsureDeleted();

            // 2. La crea de nuevo con los nuevos Seeds
            Database.EnsureCreated();
        }

        public DbSet<Receta> Recetas { get; set; }
        public DbSet<PasoReceta> PasosRecetas { get; set; }
        public DbSet<Ingrediente> Ingredientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- 1. RECETAS ---
            modelBuilder.Entity<Receta>().HasData(
                new Receta { Id = 1, Nombre = "Huevos con Chorizo" },
                new Receta { Id = 2, Nombre = "Pasta Alfredo Express" },
                new Receta { Id = 3, Nombre = "Tacos de Pollo" }
            );

            // --- 2. PASOS ---
            modelBuilder.Entity<PasoReceta>().HasData(
                // Pasos para Huevos con Chorizo (Receta 1)
                new PasoReceta { Id = 1, RecetaId = 1, Accion = "Picar el chorizo", TiempoMinutos = 5 },
                new PasoReceta { Id = 2, RecetaId = 1, Accion = "Freír y mezclar con huevo", TiempoMinutos = 10 },

                // Pasos para Pasta Alfredo (Receta 2)
                new PasoReceta { Id = 3, RecetaId = 2, Accion = "Cocer el fettuccine", TiempoMinutos = 12 },
                new PasoReceta { Id = 4, RecetaId = 2, Accion = "Preparar salsa de crema y parmesano", TiempoMinutos = 8 },

                // Pasos para Tacos de Pollo (Receta 3)
                new PasoReceta { Id = 5, RecetaId = 3, Accion = "Asar pechuga de pollo", TiempoMinutos = 15 },
                new PasoReceta { Id = 6, RecetaId = 3, Accion = "Calentar tortillas y armar", TiempoMinutos = 5 }
            );

            // --- 3. INGREDIENTES ---
            modelBuilder.Entity<Ingrediente>().HasData(
                // Ingredientes Huevos con Chorizo
                new Ingrediente { Id = 1, PasoRecetaId = 1, Nombre = "Chorizo", Cantidad = 100, Unidad = "gr" },
                new Ingrediente { Id = 2, PasoRecetaId = 2, Nombre = "Huevos", Cantidad = 2, Unidad = "pzas" },

                // Ingredientes Pasta Alfredo
                new Ingrediente { Id = 3, PasoRecetaId = 3, Nombre = "Pasta Fettuccine", Cantidad = 250, Unidad = "gr" },
                new Ingrediente { Id = 4, PasoRecetaId = 4, Nombre = "Crema de leche", Cantidad = 200, Unidad = "ml" },
                new Ingrediente { Id = 5, PasoRecetaId = 4, Nombre = "Queso Parmesano", Cantidad = 50, Unidad = "gr" },

                // Ingredientes Tacos de Pollo
                new Ingrediente { Id = 6, PasoRecetaId = 5, Nombre = "Pechuga de Pollo", Cantidad = 500, Unidad = "gr" },
                new Ingrediente { Id = 7, PasoRecetaId = 6, Nombre = "Tortillas de maíz", Cantidad = 6, Unidad = "pzas" },
                new Ingrediente { Id = 8, PasoRecetaId = 6, Nombre = "Salsa verde", Cantidad = 1, Unidad = "taza" }
            );
        }
    }
}