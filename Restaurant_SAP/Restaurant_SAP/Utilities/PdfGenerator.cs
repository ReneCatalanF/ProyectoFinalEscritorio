// PdfGenerator.cs
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Restaurant_SAP.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Restaurant_SAP.Utilities
{
    public class PdfGenerator
    {
        public static byte[] GeneratePedidosPdf(List<Pedido> pedidos)
        {
            using (var document = new PdfDocument())
            {
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page); // Inicializar gfx para la primera página

                var fontTitle = new XFont("Verdana", 20, XFontStyleEx.Bold);
                var fontHeader = new XFont("Verdana", 12, XFontStyleEx.Bold);
                var fontBody = new XFont("Verdana", 10);
                const double lineHeight = 15;
                double yPosition = 50;
                double xPosition = 50;

                gfx.DrawString("Historial de Pedidos", fontTitle, XBrushes.Black, new XPoint(xPosition, yPosition));
                yPosition += 30;

                // Headers de la tabla
                gfx.DrawString("Fecha/Hora", fontHeader, XBrushes.Black, new XPoint(xPosition, yPosition));
                xPosition += 150;
                gfx.DrawString("Mesa", fontHeader, XBrushes.Black, new XPoint(xPosition, yPosition));
                xPosition += 50;
                gfx.DrawString("Menú", fontHeader, XBrushes.Black, new XPoint(xPosition, yPosition));
                xPosition += 200;
                gfx.DrawString("Precio", fontHeader, XBrushes.Black, new XPoint(xPosition, yPosition));

                yPosition += 20;
                gfx.DrawLine(XPens.Black, 50, yPosition, page.Width - 50, yPosition);
                yPosition += 12;
                xPosition = 50; // Reset xPosition para los datos

                // Datos de los pedidos
                foreach (var pedido in pedidos)
                {
                    gfx.DrawString(pedido.FechaHora.ToString("dd/MM/yyyy HH:mm"), fontBody, XBrushes.Black, new XPoint(xPosition, yPosition));
                    xPosition += 150;
                    gfx.DrawString(pedido.Mesa?.Numero.ToString() ?? "", fontBody, XBrushes.Black, new XPoint(xPosition, yPosition));
                    xPosition += 50;
                    gfx.DrawString(pedido.Menu?.Nombre ?? "", fontBody, XBrushes.Black, new XPoint(xPosition, yPosition));
                    xPosition += 200;
                    gfx.DrawString(pedido.Precio.ToString("C"), fontBody, XBrushes.Black, new XPoint(xPosition, yPosition));

                    yPosition += lineHeight;
                    xPosition = 50; // Reset xPosition para la siguiente fila

                    if (yPosition > page.Height - 50) // Nueva página si se alcanza el final
                    {
                        page = document.AddPage();
                        gfx.Dispose(); // Liberar recursos de la página anterior
                        gfx = XGraphics.FromPdfPage(page); // Crear nuevo XGraphics para la nueva página
                        yPosition = 50;
                    }
                }

                yPosition += 10;
                gfx.DrawLine(XPens.Black, 50, yPosition, page.Width - 50, yPosition);
                yPosition += 12;
                xPosition = page.Width - 150;
                gfx.DrawString($"Total: {pedidos.Sum(p => p.Precio):C}", fontHeader, XBrushes.Black, new XPoint(xPosition, yPosition));

                gfx.Dispose(); // Asegurar la liberación de recursos del último XGraphics

                using (var stream = new MemoryStream())
                {
                    document.Save(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}