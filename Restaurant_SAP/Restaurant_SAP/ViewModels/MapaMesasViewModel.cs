using Restaurant_SAP.DB;
using Restaurant_SAP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;


namespace Restaurant_SAP.ViewModels
{
    public class MapaMesasViewModel : INotifyPropertyChanged
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private ObservableCollection<Mesa> _mesas;
        public ObservableCollection<Mesa> Mesas
        {
            get => _mesas;
            set
            {
                if (_mesas != null)
                    _mesas.CollectionChanged -= Mesas_CollectionChanged;

                _mesas = value;
                OnPropertyChanged(nameof(Mesas));
                _logger.Debug($"Colección de mesas para el mapa actualizada. Nuevo conteo: {_mesas?.Count}.");

                if (_mesas != null)
                    _mesas.CollectionChanged += Mesas_CollectionChanged;
            }
        }

        private int _cantidadMesas;
        public int CantidadMesas
        {
            get => _cantidadMesas;
            set
            {
                _cantidadMesas = value;
                OnPropertyChanged(nameof(CantidadMesas));
                _logger.Debug($"Cantidad de mesas en el mapa: {_cantidadMesas}.");
            }
        }

        public MapaMesasViewModel(MesasViewModel mesasViewModel)
        {
            _logger.Trace("Constructor MapaMesasViewModel llamado.");
            Mesas = mesasViewModel.Mesas;
            CantidadMesas = Mesas.Count;

            mesasViewModel.PropertyChanged += MesasViewModel_PropertyChanged;
            _logger.Debug("Suscrito al evento PropertyChanged de MesasViewModel.");
        }

        private void MesasViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MesasViewModel.Mesas))
            {
                OnPropertyChanged(nameof(Mesas));
                OnPropertyChanged(nameof(CantidadMesas));
                _logger.Debug("La propiedad Mesas ha cambiado en MesasViewModel. Notificando cambios.");
            }
        }

        private RelayCommand<Mesa> _seleccionarMesaCommand;
        public ICommand SeleccionarMesaCommand => _seleccionarMesaCommand ??= new RelayCommand<Mesa>(SeleccionarMesa);

        private void SeleccionarMesa(Mesa mesaSeleccionada)
        {
            Console.WriteLine("ENTRO CON LA MESA SELECCIONADA");
            _logger.Info($"Mesa seleccionada desde el mapa: {mesaSeleccionada?.Numero} (ID: {mesaSeleccionada?.Id}).");

            // Acceder al ViewModelLocator a través del DataContext del MainWindow
            if (Application.Current.MainWindow?.DataContext is ViewModelLocator locator)
            {
                // Establecer la mesa seleccionada en el MesaPedidoViewModel
                locator.MesaPedidoViewModel.SelectedMesa = mesaSeleccionada;
                _logger.Debug($"Mesa ID: {mesaSeleccionada?.Id} pasada al MesaPedidoViewModel.");

                // Cambiar la pestaña seleccionada a la de Pedidos (índice 1)
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.tabControl.SelectedIndex = 1;
                    _logger.Info("Cambiando a la pestaña de Pedidos.");
                }
                else
                {
                    _logger.Warn("No se pudo acceder a la ventana principal para cambiar la pestaña.");
                }
            }
            else
            {
                _logger.Error("No se pudo acceder al ViewModelLocator desde el DataContext del MainWindow.");
            }
        }

        private void Mesas_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Mesas));
            OnPropertyChanged(nameof(CantidadMesas));
            _logger.Debug($"La colección de mesas del mapa ha cambiado. Acción: {e.Action}.");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}