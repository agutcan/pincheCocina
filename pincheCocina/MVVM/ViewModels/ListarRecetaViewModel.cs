using System.Collections.ObjectModel;
using System.Text;
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

        // --- LÓGICA MOVIDA DE LA VISTA ---
        public string ObtenerTextoLecturaPaso(PasoReceta paso)
        {
            // Usamos StringBuilder para mayor eficiencia si el texto es largo
            var sb = new StringBuilder();

            sb.Append($"Paso: {paso.Accion}. ");

            if (paso.TiempoMinutos > 0)
            {
                sb.Append($"Tiempo estimado: {paso.TiempoMinutos} minutos. ");
            }

            if (paso.Ingredientes != null && paso.Ingredientes.Count > 0)
            {
                sb.Append("Ingredientes necesarios: ");
                foreach (var ing in paso.Ingredientes)
                {
                    // Manejamos el reemplazo de forma segura si Unidad es nulo
                    string unidadOriginal = ing.Unidad?.ToLower() ?? "";

                    string unidadLeible = unidadOriginal
                        .Replace("pzas", "piezas")
                        .Replace("pza", "pieza")
                        .Replace("gr", "gramos")
                        .Replace("g", "gramos")
                        .Replace("kg", "kilogramos")
                        .Replace("ml", "mililitros")
                        .Replace("l", "litros");

                    sb.Append($"{ing.Cantidad} {unidadLeible} de {ing.Nombre}. ");
                }
            }

            return sb.ToString();
        }
    }
}