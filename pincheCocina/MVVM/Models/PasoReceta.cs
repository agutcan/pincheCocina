using System;
using System.Collections.Generic;
using System.Text;

namespace pincheCocina.MVVM.Models
{
    public class PasoReceta
    {
        public int Id { get; set; }

        // Relación con Receta
        public int RecetaId { get; set; }
        public string Accion { get; set; }
        public List<Ingrediente> Ingredientes { get; set; } = new();

        public PasoReceta() { }

        public PasoReceta(string accion_)
        {
            Accion = accion_;
        }

        public bool AddAccion(string accion_)
        {
            if (accion_ != null)
            {
                Accion = accion_;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool addIngrediente(Ingrediente ingrediente)
        {
            if (ingrediente != null)
            {
                Ingredientes.Add(ingrediente);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
