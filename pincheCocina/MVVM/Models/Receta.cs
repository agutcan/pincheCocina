using System;
using System.Collections.Generic;
using System.Text;
using PropertyChanged;

namespace pincheCocina.MVVM.Models;

[AddINotifyPropertyChangedInterface]
public class Receta
{
<<<<<<< HEAD
<<<<<<< HEAD
    public class Receta
=======
    public int Id { get; set; } // Clave primaria
    public string Nombre { get; set; }
    public List<PasoReceta> Pasos { get; set; } = new();

    public Receta() { }

    public Receta(string nombre_)
>>>>>>> 53d43cf1a168a79dfb7c4ce3f080ca8877dab198
    {
        Nombre = nombre_;
    }

    public Receta(string nombre_, List<PasoReceta> Pasos_)
    {
        Nombre = nombre_;
        Pasos = Pasos_;
    }
<<<<<<< HEAD
=======
    public string Nombre { get; set; }
    public List<PasoReceta> Pasos { get; set; } = new();
>>>>>>> ffdf4cec6159daadb8375bd2f2e66e181a4e5828
=======
>>>>>>> 53d43cf1a168a79dfb7c4ce3f080ca8877dab198
}
