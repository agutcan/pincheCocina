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

    public bool AddIngrediente(Ingrediente ingrediente)
    {
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
