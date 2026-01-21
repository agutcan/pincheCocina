using Microsoft.EntityFrameworkCore;
using pincheCocina.Data;
using pincheCocina.MVVM.Models;
using System.Collections.ObjectModel;

namespace pincheCocina.MVVM.ViewModels
{
    public class ListarRecetaViewModel
    {
        private readonly AppDbContext _context;
        public ObservableCollection<Receta> Recetas { get; set; }

        // El constructor ahora recibe el AppDbContext automáticamente gracias a MauiProgram
        public ListarRecetaViewModel(AppDbContext context)
        {
            _context = context;
            Recetas = new ObservableCollection<Receta>();

            // Cargamos los datos
            CargarRecetas();
        }

        public void CargarRecetas()
        {
            // Limpiamos la lista actual
            Recetas.Clear();

            // Consultamos la base de datos incluyendo hijos y nietos
            var recetasDb = _context.Recetas
                .Include(r => r.Pasos) // Carga los pasos
                    .ThenInclude(p => p.Ingredientes) // Carga los ingredientes de cada paso
                .ToList();

            foreach (var receta in recetasDb)
            {
                Recetas.Add(receta);
            }
        }
    }
}