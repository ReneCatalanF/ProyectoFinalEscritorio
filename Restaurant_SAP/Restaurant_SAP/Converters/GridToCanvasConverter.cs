using System;
using System.Globalization;
using System.Windows.Data;

namespace Restaurant_SAP.Converters
{
    public class GridToCanvasConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2 || values[0] == null || values[1] == null)
            {
                return 0.0; // O algún valor por defecto o excepción
            }

            double gridCoordinate;
            double canvasSize;

            if (!double.TryParse(values[0].ToString(), out gridCoordinate) || !double.TryParse(values[1].ToString(), out canvasSize))
            {
                return 0.0; // O algún valor por defecto o excepción
            }

            if (parameter != null && parameter.ToString().StartsWith("WidthGrid="))
            {
                int gridSize = int.Parse(parameter.ToString().Substring("WidthGrid=".Length));
                return (gridCoordinate - 0.5) * (canvasSize / gridSize); // Centrar en el ancho
            }
            else if (parameter != null && parameter.ToString().StartsWith("HeightGrid="))
            {
                int gridSize = int.Parse(parameter.ToString().Substring("HeightGrid=".Length));
                return (gridCoordinate - 0.5) * (canvasSize / gridSize); // Centrar en el alto
            }

            return 0.0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ConvertBack no implementado para este converter.");
        }
    }
}