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

namespace Restaurant_SAP.ViewModels
{


    public class MapaMesasViewModel : INotifyPropertyChanged
    {
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

                if (_mesas != null)
                    _mesas.CollectionChanged += Mesas_CollectionChanged;
            }
        }

        public ObservableCollection<int> TamañosMapa { get; } = new ObservableCollection<int> { 3, 4, 5 };
        private int _tamañoSeleccionado = 4;
        public int TamañoSeleccionado
        {
            get => _tamañoSeleccionado;
            set
            {
                _tamañoSeleccionado = value;
                OnPropertyChanged(nameof(TamañoSeleccionado));
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
            }
        }

        public MapaMesasViewModel(MesasViewModel mesasViewModel)
        {
            
            Mesas = mesasViewModel.Mesas;
            CantidadMesas = Mesas.Count;

            mesasViewModel.PropertyChanged += MesasViewModel_PropertyChanged;
        }

        private void MesasViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MesasViewModel.Mesas))
            {
                OnPropertyChanged(nameof(Mesas));
                OnPropertyChanged(nameof(CantidadMesas));
            }
        }

        private void Mesas_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Mesas));
            OnPropertyChanged(nameof(CantidadMesas));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }





    }

}
