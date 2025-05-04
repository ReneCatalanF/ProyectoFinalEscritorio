using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Restaurant_SAP.Views
{
    /// <summary>
    /// Lógica de interacción para MapaMesasView.xaml
    /// </summary>
    public partial class MapaMesasView : UserControl
    {
        public MapaMesasView()
        {
            InitializeComponent();
            GridCanvas.SizeChanged += GridCanvas_SizeChanged;
            DrawGridAndCoordinates(); // Dibujar la cuadrícula inicial
        }

        private void GridCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawGridAndCoordinates();
        }

        private void DrawGridAndCoordinates()
        {
            GridCanvas.Children.Clear(); // Limpiar cualquier dibujo anterior

            int numFilas = 10;
            int numColumnas = 10;
            double canvasAncho = GridCanvas.ActualWidth;
            double canvasAlto = GridCanvas.ActualHeight;

            if (canvasAncho <= 0 || canvasAlto <= 0) return;

            double anchoCelda = canvasAncho / numColumnas;
            double altoCelda = canvasAlto / numFilas;

            // Dibujar líneas verticales y números de columna (inferior)
            for (int i = 0; i <= numColumnas; i++)
            {
                double x = i * anchoCelda;
                Line lineaVertical = new Line
                {
                    X1 = x,
                    Y1 = 0,
                    X2 = x,
                    Y2 = canvasAlto,
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 1
                };
                GridCanvas.Children.Add(lineaVertical);

                if (i > 0 && i <= numColumnas)
                {
                    TextBlock textoColumna = new TextBlock
                    {
                        Text = i.ToString(),
                        Foreground = Brushes.Gray,
                        FontSize = 10
                    };
                    textoColumna.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    Canvas.SetLeft(textoColumna, x - textoColumna.DesiredSize.Width / 2);
                    Canvas.SetTop(textoColumna, canvasAlto + 10);
                    GridCanvas.Children.Add(textoColumna);
                }
            }

            // Dibujar líneas horizontales y números de fila (izquierda)
            for (int i = 0; i <= numFilas; i++)
            {
                double y = i * altoCelda;
                Line lineaHorizontal = new Line
                {
                    X1 = 0,
                    Y1 = y,
                    X2 = canvasAncho,
                    Y2 = y,
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 1
                };
                GridCanvas.Children.Add(lineaHorizontal);

                if (i > 0 && i <= numFilas)
                {
                    TextBlock textoFila = new TextBlock
                    {
                        Text = i.ToString(),
                        Foreground = Brushes.Gray,
                        FontSize = 10
                    };
                    textoFila.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    Canvas.SetLeft(textoFila, -22);
                    Canvas.SetTop(textoFila, y - textoFila.DesiredSize.Height / 2);
                    GridCanvas.Children.Add(textoFila);
                }
            }
        }
    }
}
