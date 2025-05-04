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