using CommunityToolkit.Mvvm.ComponentModel;
using Restaurant_SAP.Commands;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using Restaurant_SAP.Properties;
using System.Diagnostics; // Importa el namespace de tus Resources

namespace Restaurant_SAP.ViewModels
{
    public partial class ConfiguracionViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isModalAbierto;

        [ObservableProperty]
        private ObservableCollection<IdiomaItem> _idiomasDisponibles;

        [ObservableProperty]
        private IdiomaItem _idiomaSeleccionado;

        public RelayCommand AbrirModalCommand { get; }
        public RelayCommand CerrarModalCommand { get; }
        public RelayCommand GuardarIdiomaCommand { get; }

        public string AlgunaPropiedad { get; } = "Prueba";

        public ConfiguracionViewModel()
        {
            AbrirModalCommand = new RelayCommand(AbrirModal);
            CerrarModalCommand = new RelayCommand(CerrarModal);
            GuardarIdiomaCommand = new RelayCommand(GuardarIdioma);

            IdiomasDisponibles = new ObservableCollection<IdiomaItem>()
            {
                new IdiomaItem { Name = "", DisplayName = "Español (Default)" }, // "" representa la cultura por defecto
                new IdiomaItem { Name = "en-US", DisplayName = "Inglés" },
                new IdiomaItem { Name = "fr-FR", DisplayName = "Francés" }
            };

            // Cargar el idioma guardado de la configuración al iniciar el ViewModel
            string idiomaGuardado = Settings.Default.IdiomaAplicacion;
            IdiomaSeleccionado = IdiomasDisponibles.FirstOrDefault(i => i.Name == idiomaGuardado) ??
                                 IdiomasDisponibles.FirstOrDefault(i => i.Name == CultureInfo.CurrentUICulture.Name) ??
                                 IdiomasDisponibles.First(); // Establecer Español como defecto si no hay guardado o coincidencia
        }

        private void AbrirModal(object parameter)
        {
            IsModalAbierto = true;
        }

        private void CerrarModal(object parameter)
        {
            IsModalAbierto = false;
        }

        private void GuardarIdioma(object parameter)
        {
            if (IdiomaSeleccionado != null)
            {
                // Obtener el idioma actualmente guardado
                string idiomaGuardado = Settings.Default.IdiomaAplicacion;

                // Verificar si el idioma seleccionado es diferente al guardado
                if (IdiomaSeleccionado.Name != idiomaGuardado)
                {
                    Settings.Default.IdiomaAplicacion = IdiomaSeleccionado.Name;
                    Settings.Default.Save();

                    // Forzar el reinicio de la aplicación
                    MessageBox.Show("La aplicación se reiniciará para aplicar el nuevo idioma.", "Cambio de Idioma", MessageBoxButton.OK);

                    // Reiniciar la aplicación usando la información del proceso actual
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = Process.GetCurrentProcess().MainModule.FileName,
                    };
                    Process.Start(startInfo);
                    Application.Current.Shutdown();
                }
                else
                {
                    MessageBox.Show("El idioma seleccionado ya está aplicado.", "Cambio de Idioma", MessageBoxButton.OK);
                }
            }
            CerrarModal(null);
        }
    }

    public class IdiomaItem
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}