using Restaurant_SAP.DB;
using Restaurant_SAP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_SAP.ViewModels
{
    public class MapaMesasViewModel : INotifyPropertyChanged
    {
        private readonly RestauranteContext _context;
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
}
