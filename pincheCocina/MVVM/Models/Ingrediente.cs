using System;
using System.Collections.Generic;
using System.Text;
using PropertyChanged;

namespace pincheCocina.MVVM.Models;

[AddINotifyPropertyChangedInterface]
public class Ingrediente
{
    public string Nombre { get; set; }
    public string Cantidad { get; set; }
    public string Unidad { get; set; }
}
