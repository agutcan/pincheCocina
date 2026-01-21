using System;
using System.Collections.Generic;
using System.Text;
using PropertyChanged;

namespace pincheCocina.MVVM.Models;

[AddINotifyPropertyChangedInterface]
public class Ingrediente
{
<<<<<<< HEAD
    public class Ingrediente
    {
        public int Id { get; set; }

        // Relación con PasoReceta
        public int PasoRecetaId { get; set; }
        public string Nombre { get; set; }
        public string Cantidad { get; set; } // ej: "2", "100"
        public string Unidad { get; set; }    // ej: "g", "ml", "unidad"

        public Ingrediente() { }

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
=======
    public string Nombre { get; set; }
    public string Cantidad { get; set; }
    public string Unidad { get; set; }
>>>>>>> ffdf4cec6159daadb8375bd2f2e66e181a4e5828
}
