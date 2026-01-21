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
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> 53d43cf1a168a79dfb7c4ce3f080ca8877dab198
        public int Id { get; set; }

        // Relación con Receta
        public int RecetaId { get; set; }
        public string Accion { get; set; }
        public List<Ingrediente> Ingredientes { get; set; } = new();

        public PasoReceta() { }

        public PasoReceta(string accion_)
<<<<<<< HEAD
=======
        if (ingrediente != null)
>>>>>>> ffdf4cec6159daadb8375bd2f2e66e181a4e5828
=======

        if (ingrediente != null)

>>>>>>> 53d43cf1a168a79dfb7c4ce3f080ca8877dab198
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
