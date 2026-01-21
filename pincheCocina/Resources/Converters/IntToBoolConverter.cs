using System.Globalization;
using pincheCocina.MVVM.Models; // Asegúrate de que este sea el namespace de tus modelos

namespace pincheCocina.Resources.Converters
{
    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Si el valor es el objeto PasoReceta completo
            if (value is PasoReceta paso)
            {
                bool tieneIngredientes = paso.Ingredientes != null && paso.Ingredientes.Count > 0;
                bool tieneTiempo = paso.TiempoMinutos > 0;
                return tieneIngredientes || tieneTiempo;
            }

            // Si el valor es una lista (Ingredientes)
            if (value is System.Collections.ICollection collection)
                return collection.Count > 0;

            // Si el valor es un número (TiempoMinutos)
            if (value is int intValue) return intValue > 0;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}