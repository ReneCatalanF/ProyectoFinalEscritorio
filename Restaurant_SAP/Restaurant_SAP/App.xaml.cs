using System.Configuration;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using Restaurant_SAP.Properties; // Asegúrate de que este namespace sea correcto
using NLog;

namespace Restaurant_SAP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public App()
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            InitializeNLog();

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
                    logger.Warn($"No se encontró la cultura guardada: {idiomaGuardado}. Se usará el idioma por defecto (es-ES).");
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
                logger.Info("No se encontró idioma guardado. Se usará el idioma por defecto (es-ES).");
                CultureInfo defaultCulture = new CultureInfo("es-ES");
                Thread.CurrentThread.CurrentCulture = defaultCulture;
                Thread.CurrentThread.CurrentUICulture = defaultCulture;
                FrameworkElement.LanguageProperty.OverrideMetadata(
                    typeof(FrameworkElement),
                    new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(defaultCulture.IetfLanguageTag)));
            }
        }
        private void InitializeNLog() // Agrega este método
        {
            logger.Info("NLog inicializado.");
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) // Agrega este método
        {
            logger.Fatal(e.Exception, "Excepción no controlada en el hilo de la interfaz de usuario.");
            e.Handled = true;
        }
    }
}