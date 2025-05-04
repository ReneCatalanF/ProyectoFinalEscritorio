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
using NLog;

namespace Restaurant_SAP.ViewModels
{
    public class ReservasViewModel : INotifyPropertyChanged
    {
        private readonly RestauranteContext _context;
        private ObservableCollection<Reserva> _reservas;
        private Reserva _reservaEnEdicion;
        private Mesa _mesaSeleccionada;
        private string _mensajeError;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

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
                _logger.Debug($"Reservas actualizadas. Nuevo conteo: {_reservas?.Count}");
            }
        }

        public Reserva ReservaEnEdicion
        {
            get => _reservaEnEdicion;
            set
            {
                _reservaEnEdicion = value;
                OnPropertyChanged(nameof(ReservaEnEdicion));
                _logger.Debug($"Reserva en edición cambiada. Nueva reserva: {_reservaEnEdicion?.Id}");
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
                _logger.Debug($"Mesa seleccionada cambiada. Nueva mesa: {_mesaSeleccionada?.Id}");
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
            _logger.Trace("Constructor ReservasViewModel llamado.");
            _context = context;
            ReservaEnEdicion = new Reserva();
            ReservaEnEdicion.FechaHoraInicio = DateTime.Now;
            ReservaEnEdicion.FechaHoraFin = DateTime.Now;
            Reservas = new ObservableCollection<Reserva>();
            Mesas = mesasViewModel.Mesas; // Asignar la MISMA INSTANCIA de la colección
            Mesas.CollectionChanged += Mesas_CollectionChanged;
            _logger.Debug($"ReservasViewModel inicializado con contexto: {_context}, MesasViewModel: {mesasViewModel}.");
        }
        private void Mesas_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Mesas));
            _logger.Debug($"La colección de mesas ha cambiado. Acción: {e.Action}.");
        }


        private void CargarReservas()
        {
            Reservas.Clear();
            _logger.Trace("Cargando reservas...");
            if (MesaSeleccionada != null)
            {
                _logger.Debug($"Cargando reservas para la mesa ID: {MesaSeleccionada.Id}");
                // Filtrar las reservas por la mesa seleccionada
                var reservasDeMesa = _context.Reservas.Where(r => r.MesaId == MesaSeleccionada.Id).Include(r => r.Mesa).ToList();
                foreach (var reserva in reservasDeMesa)
                {
                    Reservas.Add(reserva);
                }
                _logger.Info($"Se cargaron {reservasDeMesa.Count} reservas para la mesa ID: {MesaSeleccionada.Id}.");
            }
            else
            {
                _logger.Info("No se seleccionó ninguna mesa. No se cargaron reservas.");
            }
        }

        public string MensajeError
        {
            get => _mensajeError;
            set
            {
                _mensajeError = value;
                OnPropertyChanged(nameof(MensajeError));
                if (!string.IsNullOrEmpty(_mensajeError))
                {
                    _logger.Error($"Mensaje de error establecido: {_mensajeError}");
                }
            }
        }

        private void AgregarReserva(object parameter)
        {
            _logger.Trace("Método AgregarReserva llamado.");
            if (MesaSeleccionada == null && ReservaEnEdicion == null)
            {
                _logger.Warn("No se puede agregar reserva: MesaSeleccionada y ReservaEnEdicion son null.");
                return;
            }

            try
            {
                if (ReservaEnEdicion == null)
                {
                    MensajeError = "Error al editar reserva";
                    _logger.Error("Error al agregar reserva: ReservaEnEdicion es null.");
                    return;
                }
                if (ReservaEnEdicion.FechaHoraInicio > ReservaEnEdicion.FechaHoraFin)
                {
                    MensajeError = "Error al agregar reserva: La Fecha inicio no puede ser despues de la Fecha Fin.";
                    _logger.Warn("Error al agregar reserva: FechaHoraInicio es posterior a FechaHoraFin.");
                    return;
                }

                _reservaEnEdicion.MesaId = MesaSeleccionada.Id;
                _context.Reservas.Add(_reservaEnEdicion);
                _context.SaveChanges();
                _logger.Info($"Reserva agregada. ID: {_reservaEnEdicion.Id}, Mesa ID: {_reservaEnEdicion.MesaId}, Inicio: {_reservaEnEdicion.FechaHoraInicio}, Fin: {_reservaEnEdicion.FechaHoraFin}.");
                CargarReservas();
                MensajeError = "";
                ReservaEnEdicion = new Reserva();
                OnPropertyChanged(nameof(Reservas));
                _logger.Debug("Nueva instancia de ReservaEnEdicion creada.");
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al agregar menu: {ex.Message}";
                _logger.Error(ex, "Error al agregar reserva.");
            }
            _logger.Trace("Método AgregarReserva completado.");
        }

        private bool CanAgregarReserva(object parameter)
        {
            _logger.Trace($"CanAgregarReserva llamado. Retornando: true.");
            return true;
        }

        private void EliminarReserva(object parameter)
        {
            _logger.Trace("Método EliminarReserva llamado.");
            if (ReservaEnEdicion != null)
            {
                try
                {
                    var reservaAEliminar = _context.Reservas.Find(ReservaEnEdicion.Id);
                    if (reservaAEliminar != null)
                    {
                        _context.Reservas.Remove(reservaAEliminar);
                        _context.SaveChanges();
                        _logger.Info($"Reserva eliminada. ID: {reservaAEliminar.Id}, Mesa ID: {reservaAEliminar.MesaId}, Inicio: {reservaAEliminar.FechaHoraInicio}, Fin: {reservaAEliminar.FechaHoraFin}.");
                        CargarReservas();
                        ReservaEnEdicion = null;
                        _logger.Debug("ReservaEnEdicion establecida a null después de la eliminación.");
                    }
                    else
                    {
                        _logger.Warn($"No se encontró la reserva con ID: {ReservaEnEdicion.Id} para eliminar.");
                    }
                }
                catch (Exception ex)
                {
                    MensajeError = $"Error al eliminar menu: {ex.Message}";
                    _logger.Error(ex, "Error al eliminar reserva.");
                }
            }
            else
            {
                _logger.Warn("No se puede eliminar reserva: ReservaEnEdicion es null.");
            }
            _logger.Trace("Método EliminarReserva completado.");
        }

        private bool CanEliminarReserva(object parameter)
        {
            bool canExecute = ReservaEnEdicion != null;
            _logger.Trace($"CanEliminarReserva llamado. Retornando: {canExecute}.");
            return canExecute;
        }

        private void GuardarCambiosReserva(object parameter)
        {
            _logger.Trace("Método GuardarCambiosReserva llamado.");
            try
            {
                if (ReservaEnEdicion != null)
                {
                    var reservaOriginal = _context.Reservas.Find(ReservaEnEdicion.Id);

                    if (reservaOriginal != null)
                    {
                        _logger.Debug($"Guardando cambios en la reserva con ID: {ReservaEnEdicion.Id}. Inicio anterior: {reservaOriginal.FechaHoraInicio}, Fin anterior: {reservaOriginal.FechaHoraFin}. Nuevo inicio: {ReservaEnEdicion.FechaHoraInicio}, Nuevo fin: {ReservaEnEdicion.FechaHoraFin}.");
                        reservaOriginal.FechaHoraInicio = ReservaEnEdicion.FechaHoraInicio;
                        reservaOriginal.FechaHoraFin = ReservaEnEdicion.FechaHoraFin;
                        _context.SaveChanges();
                        _logger.Info($"Cambios guardados en la reserva con ID: {reservaOriginal.Id}.");
                        CargarReservas();
                        ReservaEnEdicion = null;
                        _logger.Debug("ReservaEnEdicion establecida a null después de guardar los cambios.");
                    }
                    else
                    {
                        _logger.Warn($"No se encontró la reserva con ID: {ReservaEnEdicion.Id} para guardar los cambios.");
                    }
                }
                else
                {
                    _logger.Warn("No se pueden guardar los cambios: ReservaEnEdicion es null.");
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al editar menu: {ex.Message}";
                _logger.Error(ex, "Error al guardar los cambios de la reserva.");
            }
            _logger.Trace("Método GuardarCambiosReserva completado.");
        }

        private bool CanGuardarCambiosReserva(object parameter)
        {
            bool canExecute = ReservaEnEdicion != null && ReservaEnEdicion.FechaHoraInicio < ReservaEnEdicion.FechaHoraFin;
            _logger.Trace($"CanGuardarCambiosReserva llamado. Retornando: {canExecute}.");
            return canExecute;
        }

        private void CancelarEdicionReserva(object parameter)
        {
            _logger.Trace("Método CancelarEdicionReserva llamado.");
            ReservaEnEdicion = new Reserva();
            _logger.Debug("ReservaEnEdicion restablecida a una nueva instancia.");
            _logger.Trace("Método CancelarEdicionReserva completado.");
        }
    }
}