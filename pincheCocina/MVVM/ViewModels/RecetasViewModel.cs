using pincheCocina.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace pincheCocina.MVVM.ViewModels;

public class RecetasViewModel
{
    public ObservableCollection<Receta> ListaRecetas { get; set; }
    public string ModoSeleccionado { get; set; }

    public RecetasViewModel()
    {
        // Datos de ejemplo
        ListaRecetas = new ObservableCollection<Receta>
        {
            new Receta
            {
                Nombre = "Tacos de Pollo",
                Pasos = new List<PasoReceta>
                {
                    new PasoReceta {
                        Accion = "Cocinar el pollo",
                        Ingredientes = new List<Ingrediente> {
                            new Ingrediente { Nombre="Pollo", Cantidad="500", Unidad="gr" }
                        }
                    }
                }
            }
        };
    }

}
