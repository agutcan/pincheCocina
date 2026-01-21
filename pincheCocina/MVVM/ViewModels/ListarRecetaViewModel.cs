using System.Collections.ObjectModel;
using pincheCocina.MVVM.Models;

namespace pincheCocina.MVVM.ViewModels
{
    public class ListarRecetaViewModel
    {
        public ObservableCollection<Receta> Recetas { get; set; }

        public ListarRecetaViewModel()
        {
            // Creamos datos de prueba directamente
            Recetas = new ObservableCollection<Receta>
            {
                new Receta("Tacos al Pastor")
                {
                    Pasos = new List<PasoReceta> { new PasoReceta("Marinar carne"), new PasoReceta("Asar") }
                },
                new Receta("Ensalada César")
                {
                    Pasos = new List<PasoReceta> { new PasoReceta("Cortar lechuga") }
                },
                new Receta("Pizza Casera")
                {
                    Pasos = new List<PasoReceta> { new PasoReceta("Preparar masa"), new PasoReceta("Hornear") }
                }
            };
        }
    }
}