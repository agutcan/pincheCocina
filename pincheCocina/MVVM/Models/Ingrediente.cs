using System;
using System.Collections.Generic;
using System.Text;
using PropertyChanged;

namespace pincheCocina.MVVM.Models;

[AddINotifyPropertyChangedInterface]
public class Ingrediente
{
    public int Id { get; set; }

    // Relación con PasoReceta
    public int PasoRecetaId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public Double Cantidad { get; set; } // ej: 2, 100
    public string Unidad { get; set; } = string.Empty;    // ej: "g", "ml", "unidad"

    public Ingrediente() { }

    public Ingrediente(string nombre_)
    {
        Nombre = nombre_;
    }

    public Ingrediente(string nombre_, double cantidad_)
    {
        Nombre = nombre_;
        Cantidad = cantidad_;
    }

    public Ingrediente(string nombre_, double cantidad_, string unidad_)
    {
        Nombre = nombre_;
        Cantidad = cantidad_;
        Unidad = unidad_;
    }
}
