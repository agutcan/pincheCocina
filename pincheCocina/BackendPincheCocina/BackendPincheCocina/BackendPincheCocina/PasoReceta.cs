using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendPincheCocina
{
    public class PasoReceta
    {
        public string Accion { get; set; }
        public List<Ingrediente> Ingredientes { get; set; } = new();

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
            if(ingrediente != null)
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
