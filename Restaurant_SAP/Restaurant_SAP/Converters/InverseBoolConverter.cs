using System;
using System.Globalization;
using System.Windows.Data;

namespace Restaurant_SAP.Converters // Reemplaza con el namespace de tu proyecto
{
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 1. Verificación del tipo de valor
            if (value is bool boolValue)
            {
                // 2. Inversión del valor booleano
                return !boolValue;
            }

            // 3. Manejo de valores no booleanos (opcional pero recomendado)
            // Puedes devolver un valor predeterminado o lanzar una excepción.
            // En este caso, devolvemos 'false' por defecto.
            return false;

            // Otra opción sería lanzar una excepción:
            // throw new ArgumentException("Value must be a boolean.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 4. Implementación de ConvertBack (generalmente no necesaria en este caso)
            // En la mayoría de los casos de conversión de visibilidad, no necesitas
            // la conversión inversa (de Visibility a bool). Por lo tanto, puedes
            // lanzar una excepción 'NotImplementedException'.
            throw new NotImplementedException();
        }
    }
}