using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendPincheCocina
{
    public class Ingrediente
    {
        public string Nombre { get; set; }
        public string Cantidad { get; set; } // ej: "2", "100"
        public string Unidad { get; set; }    // ej: "g", "ml", "unidad"

        public Ingrediente(string nombre_) 
        {
            Nombre = nombre_;
        }

        public Ingrediente(string nombre_, string cantidad_)
        {
            Nombre = nombre_;
            Cantidad = cantidad_;
        }

        public Ingrediente(string nombre_, string cantidad_, string unidad_)
        {
            Nombre = nombre_;
            Cantidad = cantidad_;
            Unidad = unidad_;
        }
    }
}
