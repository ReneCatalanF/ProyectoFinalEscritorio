using CommunityToolkit.Mvvm.ComponentModel;
using Restaurant_SAP.Commands;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using Restaurant_SAP.Properties;
using System.Diagnostics;
using NLog; // Importa el namespace de NLog

namespace Restaurant_SAP.ViewModels
{
    public partial class ConfiguracionViewModel : ObservableObject
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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
            logger.Trace("Constructor ConfiguracionViewModel llamado."); // Log al crear el ViewModel

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
            logger.Debug($"Idioma guardado al inicio: '{idiomaGuardado}'.");
            IdiomaSeleccionado = IdiomasDisponibles.FirstOrDefault(i => i.Name == idiomaGuardado) ??
                                 IdiomasDisponibles.FirstOrDefault(i => i.Name == CultureInfo.CurrentUICulture.Name) ??
                                 IdiomasDisponibles.First(); // Establecer Español como defecto si no hay guardado o coincidencia
            logger.Info($"Idioma seleccionado al inicio: '{IdiomaSeleccionado?.Name ?? "null"}'.");
        }

        private void AbrirModal(object parameter)
        {
            logger.Trace("Método AbrirModal llamado.");
            IsModalAbierto = true;
            logger.Debug($"IsModalAbierto ahora es: {IsModalAbierto}.");
        }

        private void CerrarModal(object parameter)
        {
            logger.Trace("Método CerrarModal llamado.");
            IsModalAbierto = false;
            logger.Debug($"IsModalAbierto ahora es: {IsModalAbierto}.");
        }

        private void GuardarIdioma(object parameter)
        {
            logger.Trace("Método GuardarIdioma llamado.");
            if (IdiomaSeleccionado != null)
            {
                logger.Debug($"Idioma seleccionado para guardar: '{IdiomaSeleccionado.Name}'.");
                // Obtener el idioma actualmente guardado
                string idiomaGuardado = Settings.Default.IdiomaAplicacion;
                logger.Debug($"Idioma guardado actualmente en la configuración: '{idiomaGuardado}'.");

                // Verificar si el idioma seleccionado es diferente al guardado
                if (IdiomaSeleccionado.Name != idiomaGuardado)
                {
                    logger.Info($"El idioma ha cambiado de '{idiomaGuardado}' a '{IdiomaSeleccionado.Name}'. Guardando configuración y reiniciando la aplicación.");
                    Settings.Default.IdiomaAplicacion = IdiomaSeleccionado.Name;
                    Settings.Default.Save();
                    logger.Debug($"Nuevo idioma '{IdiomaSeleccionado.Name}' guardado en la configuración.");

                    MessageBox.Show("La aplicación se reiniciará para aplicar el nuevo idioma.", "Cambio de Idioma", MessageBoxButton.OK);

                    // Reiniciar la aplicación usando la información del proceso actual
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = Process.GetCurrentProcess().MainModule.FileName,
                    };
                    logger.Info($"Iniciando nuevo proceso: '{startInfo.FileName}'.");
                    Process.Start(startInfo);
                    Application.Current.Shutdown();
                    logger.Info("Aplicación actual cerrándose.");
                }
                else
                {
                    logger.Info($"El idioma seleccionado '{IdiomaSeleccionado.Name}' ya estaba aplicado. No se requiere acción.");
                    MessageBox.Show("El idioma seleccionado ya está aplicado.", "Cambio de Idioma", MessageBoxButton.OK);
                }
            }
            else
            {
                logger.Warn("IdiomaSeleccionado es null. No se puede guardar el idioma.");
            }
            CerrarModal(null);
            logger.Trace("Método GuardarIdioma completado.");
        }
    }

    public class IdiomaItem
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}