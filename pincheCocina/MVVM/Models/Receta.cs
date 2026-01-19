using System;
using System.Collections.Generic;
using System.Text;

namespace pincheCocina.MVVM.Models
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
