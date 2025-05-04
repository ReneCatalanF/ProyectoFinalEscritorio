using System.Configuration;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using Restaurant_SAP.Properties; // Asegúrate de que este namespace sea correcto

namespace Restaurant_SAP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // Cargar el idioma guardado al inicio de la aplicación
            string idiomaGuardado = Settings.Default.IdiomaAplicacion;

            if (!string.IsNullOrEmpty(idiomaGuardado))
            {
                try
                {
                    CultureInfo culture = new CultureInfo(idiomaGuardado);
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;

                    // Forzar la actualización de la cultura para WPF
                    FrameworkElement.LanguageProperty.OverrideMetadata(
                        typeof(FrameworkElement),
                        new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(culture.IetfLanguageTag)));
                }
                catch (CultureNotFoundException)
                {
                    // Manejar el caso en que el idioma guardado no sea válido
                    // Puedes establecer un idioma por defecto aquí si lo deseas
                    CultureInfo defaultCulture = new CultureInfo("es-ES");
                    Thread.CurrentThread.CurrentCulture = defaultCulture;
                    Thread.CurrentThread.CurrentUICulture = defaultCulture;
                    FrameworkElement.LanguageProperty.OverrideMetadata(
                        typeof(FrameworkElement),
                        new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(defaultCulture.IetfLanguageTag)));
                }
            }
            else
            {
                // Si no hay idioma guardado, establecer el idioma por defecto (Español)
                CultureInfo defaultCulture = new CultureInfo("es-ES");
                Thread.CurrentThread.CurrentCulture = defaultCulture;
                Thread.CurrentThread.CurrentUICulture = defaultCulture;
                FrameworkElement.LanguageProperty.OverrideMetadata(
                    typeof(FrameworkElement),
                    new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(defaultCulture.IetfLanguageTag)));
            }
        }
    }
}