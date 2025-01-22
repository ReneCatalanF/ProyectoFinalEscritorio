using Microsoft.EntityFrameworkCore;
using Restaurant_SAP.Commands;
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
using System.Windows.Input;

namespace Restaurant_SAP.ViewModels
{
    public class ReservasViewModel : INotifyPropertyChanged
    {
        private readonly RestauranteContext _context;
        private ObservableCollection<Reserva> _reservas;
        private Reserva _reservaEnEdicion;
        private Mesa _mesaSeleccionada;
        private string _mensajeError;

        public ObservableCollection<Mesa> Mesas { get; private set; }

        private RelayCommand _agregarReservaCommand;
        private RelayCommand _eliminarReservaCommand;
        private RelayCommand _guardarCambiosReservaCommand;
        private RelayCommand _cancelarEdicionReservaCommand;

        public ObservableCollection<Reserva> Reservas
        {
            get => _reservas;
            set
            {
                _reservas = value;
                OnPropertyChanged(nameof(Reservas));
            }
        }

        public Reserva ReservaEnEdicion
        {
            get => _reservaEnEdicion;
            set
            {
                _reservaEnEdicion = value;
                OnPropertyChanged(nameof(ReservaEnEdicion));
                _guardarCambiosReservaCommand?.RaiseCanExecuteChanged();
                _eliminarReservaCommand?.RaiseCanExecuteChanged();
            }
        }

        public Mesa MesaSeleccionada
        {
            get => _mesaSeleccionada;
            set
            {
                _mesaSeleccionada = value;
                OnPropertyChanged(nameof(MesaSeleccionada));
                CargarReservas();
                _agregarReservaCommand?.RaiseCanExecuteChanged();
            }
        }

        public ICommand AgregarReservaCommand => _agregarReservaCommand ??= new RelayCommand(AgregarReserva, CanAgregarReserva);
        public ICommand EliminarReservaCommand => _eliminarReservaCommand ??= new RelayCommand(EliminarReserva, CanEliminarReserva);
        public ICommand GuardarCambiosReservaCommand => _guardarCambiosReservaCommand ??= new RelayCommand(GuardarCambiosReserva, CanGuardarCambiosReserva);
        public ICommand CancelarEdicionReservaCommand => _cancelarEdicionReservaCommand ??= new RelayCommand(CancelarEdicionReserva);

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ReservasViewModel(RestauranteContext context, MesasViewModel mesasViewModel)
        {
            _context = context;
            ReservaEnEdicion = new Reserva();
            ReservaEnEdicion.FechaHoraInicio = DateTime.Now;
            ReservaEnEdicion.FechaHoraFin = DateTime.Now;
            Reservas = new ObservableCollection<Reserva>();
            Mesas = mesasViewModel.Mesas; // Asignar la MISMA INSTANCIA de la colección
            Mesas.CollectionChanged += Mesas_CollectionChanged;


        }
        private void Mesas_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Mesas));
        }


        private void CargarReservas()
        {
            Reservas.Clear();
            if (MesaSeleccionada != null)
            {
                // Filtrar las reservas por la mesa seleccionada
                var reservasDeMesa = _context.Reservas.Where(r => r.MesaId == MesaSeleccionada.Id).Include(r => r.Mesa).ToList();
                foreach (var reserva in reservasDeMesa)
                {
                    Reservas.Add(reserva);
                }
            }
        }

        public string MensajeError
        {
            get => _mensajeError;
            set
            {
                _mensajeError = value;
                OnPropertyChanged(nameof(MensajeError));
            }
        }

        private void AgregarReserva(object parameter)
        {
            if (MesaSeleccionada == null && ReservaEnEdicion == null) return;


            try
            {
                if (ReservaEnEdicion == null) {
                    MensajeError = "Error al editar reserva";
                    return;
                }
                if (ReservaEnEdicion.FechaHoraInicio > ReservaEnEdicion.FechaHoraFin) {
                    MensajeError = "Error al agregar reserva: La Fecha inicio no puede ser despues de la Fecha Fin.";
                    return;
                }


                _reservaEnEdicion.MesaId = MesaSeleccionada.Id;
                _context.Reservas.Add(_reservaEnEdicion);
                _context.SaveChanges();
                CargarReservas();
                MensajeError = "";
                ReservaEnEdicion = new Reserva() ;
                OnPropertyChanged(nameof(Reservas));

            }
            catch (Exception ex)
            {
                MensajeError = $"Error al agregar menu: {ex.Message}";
            }
        }

        private bool CanAgregarReserva(object parameter)
        {
            return true;
        }

        private void EliminarReserva(object parameter)
        {
            if (ReservaEnEdicion != null)
            {
                try
                {
                    var reservaAEliminar = _context.Reservas.Find(ReservaEnEdicion.Id);
                    if (reservaAEliminar != null)
                    {
                        _context.Reservas.Remove(reservaAEliminar);
                        _context.SaveChanges();
                        CargarReservas();
                        ReservaEnEdicion = null;
                    }
                }
                catch (Exception ex)
                {
                    MensajeError = $"Error al eliminar menu: {ex.Message}";
                }
            }
        }

        private bool CanEliminarReserva(object parameter)
        {
            return ReservaEnEdicion != null;
        }

        private void GuardarCambiosReserva(object parameter)
        {
            try
            {
                if (ReservaEnEdicion != null)
                {
                    var reservaOriginal = _context.Reservas.Find(ReservaEnEdicion.Id);

                    if (reservaOriginal != null)
                    {
                        reservaOriginal.FechaHoraInicio = ReservaEnEdicion.FechaHoraInicio;
                        reservaOriginal.FechaHoraFin = ReservaEnEdicion.FechaHoraFin;
                        _context.SaveChanges();
                        CargarReservas();
                        ReservaEnEdicion = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al editar menu: {ex.Message}";
            }
        }

        private bool CanGuardarCambiosReserva(object parameter)
        {
            return ReservaEnEdicion != null && ReservaEnEdicion.FechaHoraInicio < ReservaEnEdicion.FechaHoraFin;
        }

        private void CancelarEdicionReserva(object parameter)
        {
            ReservaEnEdicion = new Reserva();
        }
    }
}
