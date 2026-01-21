using System;
using System.Collections.Generic;
using System.Text;
using PropertyChanged;

namespace pincheCocina.MVVM.Models;

[AddINotifyPropertyChangedInterface]
public class Receta
{
    public string Nombre { get; set; }
    public List<PasoReceta> Pasos { get; set; } = new();
}
