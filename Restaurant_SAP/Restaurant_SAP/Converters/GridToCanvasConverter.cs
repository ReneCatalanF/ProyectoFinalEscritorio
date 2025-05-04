using System;
using System.Globalization;
using System.Windows.Data;

namespace Restaurant_SAP.Converters
{
    public class GridToCanvasConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2 || values[0] == null || values[1] == null || parameter == null)
            {
                return 0.0;
            }

            double gridCoordinate;
            double canvasSize;
            int gridSize;
            string parameterString = parameter.ToString();
            string[] parts = parameterString.Split('=');

            if (!double.TryParse(values[0].ToString(), out gridCoordinate) || !double.TryParse(values[1].ToString(), out canvasSize) || parts.Length != 2 || !int.TryParse(parts[1], out gridSize))
            {
                return 0.0;
            }

            // Considerar el margen de la mesa (10 en total: 5 a la izquierda y 5 a la derecha/arriba/abajo)
            double margenMesa = 10;
            double espacioDisponibleCelda = (canvasSize / gridSize) - margenMesa;
            double inicioCelda = (canvasSize / gridSize) / 2; // Punto central de la celda

            if (parts[0].ToLower() == "widthgrid")
            {
                // Calcular la posición centrada dentro de la celda, considerando el margen
                return (gridCoordinate - 0.5) * (canvasSize / gridSize) - (espacioDisponibleCelda / 2);
            }
            else if (parts[0].ToLower() == "heightgrid")
            {
                return (gridCoordinate - 0.5) * (canvasSize / gridSize) - (espacioDisponibleCelda / 2);
            }

            return 0.0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ConvertBack no implementado para este converter.");
        }
    }
}