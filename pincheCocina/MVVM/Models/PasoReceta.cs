using System;
using System.Collections.Generic;
using System.Text;
using PropertyChanged;

namespace pincheCocina.MVVM.Models;

[AddINotifyPropertyChangedInterface]
public class PasoReceta
{
    public string Accion { get; set; }
    public List<Ingrediente> Ingredientes { get; set; } = new();

    public int Id { get; set; }

    // Relación con Receta
    public int RecetaId { get; set; }

    public int? TiempoMinutos { get; set; } // Propiedad nueva

    // Propiedad calculada para la vista
    public bool TieneTiempo => TiempoMinutos.HasValue && TiempoMinutos > 0;

    public PasoReceta() { }

    public PasoReceta(string accion, int tiempo)
    {
        Accion = accion;
        TiempoMinutos = tiempo;
    }

    public bool AddIngrediente(Ingrediente ingrediente) {
        if (ingrediente != null)

        {
            Ingredientes.Add(ingrediente);
            return true;
        }
        else
            return false;
    }

    public bool RemoveIngrediente(Ingrediente ingrediente)
    {
        return (ingrediente != null) ? Ingredientes.Remove(ingrediente) : false;
    }

}
