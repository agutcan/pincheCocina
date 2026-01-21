using System;
using System.Collections.Generic;
using System.Text;
using PropertyChanged;

namespace pincheCocina.MVVM.Models;

[AddINotifyPropertyChangedInterface]
public class Receta
{
<<<<<<< HEAD
    public class Receta
    {
        public int Id { get; set; } // Clave primaria
        public string Nombre { get; set; }
        public List<PasoReceta> Pasos { get; set; } = new();

        public Receta() { }

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
=======
    public string Nombre { get; set; }
    public List<PasoReceta> Pasos { get; set; } = new();
>>>>>>> ffdf4cec6159daadb8375bd2f2e66e181a4e5828
}
