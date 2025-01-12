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
    
    /*
    public class MapaMesasViewModel : INotifyPropertyChanged
    {
        private RestauranteContext _context;
        public ObservableCollection<Mesa> Mesas { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string CantidadMesas => $"Cantidad de Mesas: {Mesas?.Count ?? 0}";

        public MapaMesasViewModel()
        {
            _context = new RestauranteContext();
            Mesas = new ObservableCollection<Mesa>(); // Inicializar la colección UNA SOLA VEZ
            CargarMesas();

            MesasViewModel.MesasChanged += CargarMesas; // Suscribir al evento
        }

        private void CargarMesas()
        {
            //Obtener los datos desde la base de datos
            var mesasDesdeBD = _context.Mesas.ToList();

            //Limpiar la coleccion actual para evitar duplicados
            Mesas.Clear();

            //Agregar los elementos a la coleccion existente, esto notifica los cambios a la UI
            foreach (var mesa in mesasDesdeBD)
            {
                Mesas.Add(mesa);
            }
            OnPropertyChanged(nameof(CantidadMesas));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    */


    
    public class MapaMesasViewModel : INotifyPropertyChanged
    {
        private readonly RestauranteContext _context;
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

        public string CantidadMesas => $"Cantidad de Mesas: {Mesas?.Count ?? 0}";

        public MapaMesasViewModel(MesasViewModel mesasViewModel)
        {
            _context = new RestauranteContext(); //Este contexto no se usa, se podria quitar. Lo dejo para que se vea la inyeccion del viewmodel
            Mesas = mesasViewModel.Mesas;
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
    }
    

    
}
