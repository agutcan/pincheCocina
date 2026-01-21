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
                .AsNoTracking() // Evita conflictos de memoria al listar
                .ToListAsync();
        }

        public async Task AddRecetaAsync(Receta receta)
        {
            if (receta == null) return;

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
            if (paso == null) return;

            paso.RecetaId = recetaId;
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
            if (ingrediente == null) return;

            ingrediente.PasoRecetaId = pasoId;
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

        // --- ACTUALIZACIÓN (MÉTODO CRÍTICO CORREGIDO) ---
        public async Task UpdateRecetaAsync(Receta receta)
        {
            if (receta == null) return;

            // 1. Limpiamos el rastro de la Receta principal en memoria local
            var local = _context.Recetas.Local.FirstOrDefault(r => r.Id == receta.Id);
            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }

            // 2. Limpiamos el rastro de todos los hijos (Pasos e Ingredientes)
            // Esto soluciona el error "instance is already being tracked"
            if (receta.Pasos != null)
            {
                foreach (var paso in receta.Pasos)
                {
                    var localPaso = _context.PasosRecetas.Local.FirstOrDefault(p => p.Id == paso.Id);
                    if (localPaso != null)
                        _context.Entry(localPaso).State = EntityState.Detached;

                    if (paso.Ingredientes != null)
                    {
                        foreach (var ing in paso.Ingredientes)
                        {
                            var localIng = _context.Ingredientes.Local.FirstOrDefault(i => i.Id == ing.Id);
                            if (localIng != null)
                                _context.Entry(localIng).State = EntityState.Detached;
                        }
                    }
                }
            }

            // 3. Actualizamos y guardamos
            _context.Recetas.Update(receta);
            await _context.SaveChangesAsync();
        }

        public async Task<Receta> GetRecetaByIdAsync(int id)
        {
            return await _context.Recetas
                .Include(r => r.Pasos)
                .ThenInclude(p => p.Ingredientes)
                .AsNoTracking() // Vital para que al editar no haya bloqueos
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}