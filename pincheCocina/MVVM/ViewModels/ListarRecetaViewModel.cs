using System.Collections.ObjectModel;
using System.Collections.Generic;
using pincheCocina.MVVM.Models;

namespace pincheCocina.MVVM.ViewModels
{
    public class ListarRecetaViewModel
    {
        // Usamos "Recetas" para que coincida con el Binding de la vista
        public ObservableCollection<Receta> Recetas { get; set; }

        public ListarRecetaViewModel()
        {
            Recetas = new ObservableCollection<Receta>
            {
                // Combinación de los datos de ambos ViewModels
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
                },
                new Receta("Tacos al Pastor")
                {
                    Pasos = new List<PasoReceta>
                    {
                        new PasoReceta {
                            Accion = "Marinar carne",
                            Ingredientes = new List<Ingrediente> {
                                new Ingrediente { Nombre="Cerdo", Cantidad="1", Unidad="kg" },
                                new Ingrediente { Nombre="Achiote", Cantidad="50", Unidad="gr" }
                            }
                        },
                        new PasoReceta("Asar", 30)
                    }
                },
                new Receta("Ensalada César")
                {
                    Pasos = new List<PasoReceta>
                    {
                        new PasoReceta {
                            Accion = "Cortar lechuga",
                            Ingredientes = new List<Ingrediente> {
                                new Ingrediente { Nombre="Lechuga Orejona", Cantidad="1", Unidad="pza" }
                            }
                        }
                    }
                },
                new Receta("Pizza Casera")
                {
                    Pasos = new List<PasoReceta>
                    {
                        new PasoReceta("Preparar masa", 20),
                        new PasoReceta("Hornear", 40)
                    }
                }
            };
        }
    }
}