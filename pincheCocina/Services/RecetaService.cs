using Microsoft.EntityFrameworkCore;
using pincheCocina.Data;
using pincheCocina.MVVM.Models;

namespace pincheCocina.Services
{
    public class RecetaService : IRecetaService
    {
        private readonly AppDbContext _context;

        public RecetaService(AppDbContext context)
        {
            _context = context;
        }

        // --- RECETAS ---
        public async Task<List<Receta>> GetRecetasAsync()
        {
            return await _context.Recetas
                .Include(r => r.Pasos)
                .ThenInclude(p => p.Ingredientes)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddRecetaAsync(Receta receta)
        {
            _context.Recetas.Add(receta);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRecetaAsync(int id)
        {
            var receta = await _context.Recetas.FindAsync(id);
            if (receta != null)
            {
                _context.Recetas.Remove(receta);
                await _context.SaveChangesAsync();
            }
        }

        // --- PASOS ---
        public async Task AddPasoAsync(int recetaId, PasoReceta paso)
        {
            paso.RecetaId = recetaId; // Vinculamos al padre
            _context.PasosRecetas.Add(paso);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePasoAsync(int pasoId)
        {
            var paso = await _context.PasosRecetas.FindAsync(pasoId);
            if (paso != null)
            {
                _context.PasosRecetas.Remove(paso);
                await _context.SaveChangesAsync();
            }
        }

        // --- INGREDIENTES ---
        public async Task AddIngredienteAsync(int pasoId, Ingrediente ingrediente)
        {
            ingrediente.PasoRecetaId = pasoId; // Vinculamos al paso
            _context.Ingredientes.Add(ingrediente);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteIngredienteAsync(int ingredienteId)
        {
            var ingrediente = await _context.Ingredientes.FindAsync(ingredienteId);
            if (ingrediente != null)
            {
                _context.Ingredientes.Remove(ingrediente);
                await _context.SaveChangesAsync();
            }
        }

        // Método genérico para actualizar cualquier cambio en el grafo
        public async Task UpdateRecetaAsync(Receta receta)
        {
            _context.Update(receta);
            await _context.SaveChangesAsync();
        }

        public async Task<Receta> GetRecetaByIdAsync(int id)
        {
            return await _context.Recetas
                .Include(r => r.Pasos)
                .ThenInclude(p => p.Ingredientes)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}