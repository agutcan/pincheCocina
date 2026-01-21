using pincheCocina.MVVM.Models;

namespace pincheCocina.Services
{
    public interface IRecetaService
    {
        // Gestión de Recetas
        Task<List<Receta>> GetRecetasAsync();
        Task<Receta> GetRecetaByIdAsync(int id);
        Task AddRecetaAsync(Receta receta);
        Task UpdateRecetaAsync(Receta receta);
        Task DeleteRecetaAsync(int id);

        // Gestión de Pasos
        Task AddPasoAsync(int recetaId, PasoReceta paso);
        Task DeletePasoAsync(int pasoId);

        // Gestión de Ingredientes
        Task AddIngredienteAsync(int pasoId, Ingrediente ingrediente);
        Task DeleteIngredienteAsync(int ingredienteId);
    }
}