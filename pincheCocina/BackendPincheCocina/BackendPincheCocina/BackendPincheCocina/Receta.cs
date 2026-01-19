using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendPincheCocina
{
    public class Receta
    {
        public string Nombre { get; set; }
        public List<PasoReceta> Pasos { get; set; } = new();

        public Receta(string nombre_) 
        {
            Nombre = nombre_;
        }

        public Receta(string nombre_, List<PasoReceta> Pasos_) 
        {
            Nombre = nombre_;
            Pasos = Pasos_;
        }



    }
}
