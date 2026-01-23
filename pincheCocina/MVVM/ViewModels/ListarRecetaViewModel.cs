using System.Collections.ObjectModel;
using pincheCocina.MVVM.Models;
using pincheCocina.Services;

namespace pincheCocina.MVVM.ViewModels
{
    public class ListarRecetaViewModel
    {
        private readonly IRecetaService _recetaService;

        // Propiedad simple para guardar "mano" o "micro"
        public string ModoSeleccionado { get; set; }

        public ObservableCollection<Receta> Recetas { get; set; } = new();

        public ListarRecetaViewModel(IRecetaService recetaService)
        {
            _recetaService = recetaService;
        }

        public async Task CargarRecetasAsync()
        {
            var lista = await _recetaService.GetRecetasAsync();
            Recetas.Clear();
            foreach (var receta in lista)
            {
                Recetas.Add(receta);
            }
        }

        public async Task EliminarRecetaAsync(int id)
        {
            await _recetaService.DeleteRecetaAsync(id);
        }
    }
}