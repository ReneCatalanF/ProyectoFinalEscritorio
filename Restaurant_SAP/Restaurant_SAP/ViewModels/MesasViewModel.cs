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
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using NLog;

namespace Restaurant_SAP.ViewModels
{


    public class MesasViewModel : INotifyPropertyChanged
    {
        private readonly RestauranteContext _context;
        private Mesa _selectedMesa;
        private Mesa _mesaEnEdicion;
        private string _mensajeError;
        private ObservableCollection<Mesa> _mesas;
        private Mesa _nuevaMesa = new Mesa();
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private DispatcherTimer _timer;

        public bool IsEditing { get; set; }

        public ObservableCollection<EstadoMesa> EstadosMesa { get; } = new ObservableCollection<EstadoMesa>(Enum.GetValues(typeof(EstadoMesa)).Cast<EstadoMesa>());

        public ObservableCollection<Mesa> Mesas
        {
            get => _mesas;
            set
            {
                if (_mesas != null)
                    _mesas.CollectionChanged -= Mesas_CollectionChanged;

                _mesas = value;
                OnPropertyChanged(nameof(Mesas));
                _logger.Debug($"La colección de mesas ha sido actualizada. Nuevo conteo: {_mesas?.Count}");

                if (_mesas != null)
                    _mesas.CollectionChanged += Mesas_CollectionChanged;
            }
        }

        private void Mesas_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Mesas));
            _logger.Debug($"La colección de mesas ha cambiado. Acción: {e.Action}.");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Mesa SelectedMesa
        {
            get => _selectedMesa;
            set
            {
                _selectedMesa = value;
                OnPropertyChanged(nameof(SelectedMesa));
                _logger.Debug($"Mesa seleccionada cambiada. Nueva mesa: {_selectedMesa?.Id}");
                _eliminarMesaCommand?.RaiseCanExecuteChanged();

                if (SelectedMesa != null)
                {
                    // Crear una NUEVA instancia de Mesa para la edición
                    MesaEnEdicion = new Mesa
                    {
                        Id = SelectedMesa.Id,
                        Numero = SelectedMesa.Numero,
                        Descripcion = SelectedMesa.Descripcion,
                        Estado = SelectedMesa.Estado,
                        CoordX = SelectedMesa.CoordX,
                        CoordY = SelectedMesa.CoordY
                    };
                    IsEditing = true;
                    _logger.Debug($"Iniciando edición de la mesa ID: {SelectedMesa.Id}.");
                }
                else
                {
                    MesaEnEdicion = null;
                    IsEditing = false;
                    _logger.Debug("Edición de mesa cancelada o no se seleccionó ninguna mesa.");
                }
            }
        }

        public Mesa MesaEnEdicion
        {
            get => _mesaEnEdicion;
            set
            {
                _mesaEnEdicion = value;
                OnPropertyChanged(nameof(MesaEnEdicion));
                _logger.Debug($"Mesa en edición cambiada. Nueva mesa en edición: {_mesaEnEdicion?.Id}");
                _guardarCambiosCommand?.RaiseCanExecuteChanged();
            }
        }

        // Campos privados para los comandos
        private RelayCommand _agregarMesaCommand;
        private RelayCommand _eliminarMesaCommand;
        private RelayCommand _guardarCambiosCommand;
        private RelayCommand _cancelarEdicionCommand;

        // Propiedades públicas que devuelven los comandos (inicialización perezosa)
        public ICommand AgregarMesaCommand => _agregarMesaCommand ??= new RelayCommand(AgregarMesa, CanAgregarMesa);
        public ICommand EliminarMesaCommand => _eliminarMesaCommand ??= new RelayCommand(EliminarMesa, CanEliminarMesa);
        public ICommand GuardarCambiosCommand => _guardarCambiosCommand ??= new RelayCommand(GuardarCambios, CanGuardarCambios);
        public ICommand CancelarEdicionCommand => _cancelarEdicionCommand ??= new RelayCommand(CancelarEdicion);


        public Mesa NuevaMesa
        {
            get => _nuevaMesa;
            set
            {
                _nuevaMesa = value;
                OnPropertyChanged(nameof(NuevaMesa));
                _logger.Debug($"Nueva mesa creada o modificada: Número: {_nuevaMesa?.Numero}, Descripción: {_nuevaMesa?.Descripcion}");
                _agregarMesaCommand?.RaiseCanExecuteChanged();
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

        public MesasViewModel(RestauranteContext context)
        {
            _logger.Trace("Constructor MesasViewModel llamado.");
            _context = context;
            Mesas = new ObservableCollection<Mesa>();
            CargarMesas();
            IniciarTimer();
            _logger.Debug($"MesasViewModel inicializado con contexto: {_context}.");
        }
        private void CargarMesas()
        {
            Mesas.Clear();
            _logger.Trace("Cargando mesas desde la base de datos...");
            var mesasDesdeBD = _context.Mesas.ToList();
            foreach (var mesa in mesasDesdeBD)
            {
                Mesas.Add(mesa);
            }
            _logger.Info($"Se cargaron {mesasDesdeBD.Count} mesas desde la base de datos.");
        }

        private void AgregarMesa(object parameter)
        {
            _logger.Trace("Método AgregarMesa llamado.");
            try
            {
                if (string.IsNullOrEmpty(NuevaMesa.Descripcion))
                {
                    MensajeError = "La descripción no puede estar vacía.";
                    _logger.Warn("Intento de agregar mesa con descripción vacía.");
                    return;
                }
                if (NuevaMesa.Numero <= 0)
                {
                    MensajeError = "El número de mesa debe ser mayor que cero.";
                    _logger.Warn($"Intento de agregar mesa con número no válido: {NuevaMesa.Numero}.");
                    return;
                }
                if (_context.Mesas.Any(m => m.Numero == NuevaMesa.Numero))
                {
                    MensajeError = "Ya existe una mesa con ese número.";
                    _logger.Warn($"Intento de agregar mesa con número duplicado: {NuevaMesa.Numero}.");
                    return;
                }

                _context.Mesas.Add(NuevaMesa);
                _context.SaveChanges();
                _logger.Info($"Mesa agregada. Número: {NuevaMesa.Numero}, Descripción: {NuevaMesa.Descripcion}, Estado: {NuevaMesa.Estado}.");
                CargarMesas();
                OnPropertyChanged(nameof(Mesas));
                NuevaMesa = new Mesa();
                MensajeError = "";
                _logger.Debug("NuevaMesa restablecida.");
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al agregar mesa: {ex.Message}";
                _logger.Error(ex, "Error al agregar mesa.");
            }
            _logger.Trace("Método AgregarMesa completado.");
        }

        private void EliminarMesa(object parameter)
        {
            _logger.Trace("Método EliminarMesa llamado.");
            try
            {
                if (SelectedMesa != null)
                {
                    // Encuentra la mesa en el contexto usando su Id
                    var mesaAEliminar = _context.Mesas.Find(SelectedMesa.Id);

                    if (mesaAEliminar != null) // Verifica si la mesa existe en la base de datos
                    {
                        _context.Mesas.Remove(mesaAEliminar);
                        _context.SaveChanges();
                        _logger.Info($"Mesa eliminada. ID: {mesaAEliminar.Id}, Número: {mesaAEliminar.Numero}, Descripción: {mesaAEliminar.Descripcion}.");
                        CargarMesas(); // Recargar las mesas desde la base de datos
                        SelectedMesa = null; // Limpiar la selección
                        MesaEnEdicion = null;
                        MensajeError = "";
                        _logger.Debug("Selección y edición de mesa restablecidas después de la eliminación.");
                    }
                    else
                    {
                        MensajeError = "La mesa ya no existe en la base de datos.";
                        _logger.Warn($"Intento de eliminar una mesa que ya no existe en la base de datos. ID: {SelectedMesa.Id}.");
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                MensajeError = $"Error al eliminar la mesa (problema de dependencias): {ex.Message}";
                _logger.Error(ex, $"Error al eliminar la mesa ID: {SelectedMesa?.Id} debido a dependencias.");
                // Puedes agregar un manejo más específico de la excepción, como mostrar un mensaje diferente al usuario.
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al eliminar la mesa: {ex.Message}";
                _logger.Error(ex, $"Error inesperado al eliminar la mesa ID: {SelectedMesa?.Id}.");
            }
            _logger.Trace("Método EliminarMesa completado.");
        }

        private bool CanEliminarMesa(object parameter)
        {
            bool canExecute = SelectedMesa != null;
            _logger.Trace($"CanEliminarMesa llamado. Retornando: {canExecute}.");
            return canExecute;
        }

        private void GuardarCambios(object parameter)
        {
            _logger.Trace("Método GuardarCambios llamado.");
            try
            {
                if (MesaEnEdicion != null)
                {
                    var mesaOriginal = _context.Mesas.Find(SelectedMesa.Id);

                    if (mesaOriginal != null)
                    {
                        _logger.Debug($"Guardando cambios en la mesa ID: {SelectedMesa.Id}. Número anterior: {mesaOriginal.Numero}, Nuevo número: {MesaEnEdicion.Numero}. Descripción anterior: {mesaOriginal.Descripcion}, Nueva descripción: {MesaEnEdicion.Descripcion}. Estado anterior: {mesaOriginal.Estado}, Nuevo estado: {MesaEnEdicion.Estado}.");
                        mesaOriginal.Numero = MesaEnEdicion.Numero;
                        mesaOriginal.Descripcion = MesaEnEdicion.Descripcion;
                        mesaOriginal.Estado = MesaEnEdicion.Estado;
                        mesaOriginal.CoordX = MesaEnEdicion.CoordX;
                        mesaOriginal.CoordY = MesaEnEdicion.CoordY;

                        _context.SaveChanges();
                        _logger.Info($"Cambios guardados en la mesa ID: {mesaOriginal.Id}.");
                        CargarMesas();
                        IsEditing = false;
                        SelectedMesa = null;
                        MesaEnEdicion = null;
                        _logger.Debug("Edición completada. Selección y mesa en edición restablecidas.");
                    }
                    else
                    {
                        _logger.Warn($"No se encontró la mesa con ID: {SelectedMesa.Id} para guardar los cambios.");
                    }
                }
                else
                {
                    _logger.Warn("No se pueden guardar los cambios: MesaEnEdicion es null.");
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al guardar cambios: {ex.Message}";
                _logger.Error(ex, $"Error al guardar cambios en la mesa ID: {SelectedMesa?.Id}.");
            }
            _logger.Trace("Método GuardarCambios completado.");
        }
        private bool CanAgregarMesa(object parameter)
        {
            _logger.Trace($"CanAgregarMesa llamado. Retornando: true.");
            return true;
        }

        private bool CanGuardarCambios(object parameter)
        {
            bool canExecute = MesaEnEdicion != null &&
                               MesaEnEdicion.Numero > 0 &&
                               !string.IsNullOrEmpty(MesaEnEdicion.Descripcion) &&
                               !Mesas.Any(m => m.Numero == MesaEnEdicion.Numero && m.Id != MesaEnEdicion.Id);
            _logger.Trace($"CanGuardarCambios llamado. Retornando: {canExecute}.");
            return canExecute;
        }

        private void CancelarEdicion(object parameter)
        {
            _logger.Trace("Método CancelarEdicion llamado.");
            IsEditing = false;
            MesaEnEdicion = null;
            SelectedMesa = null;
            _logger.Debug("Edición cancelada. Selección y mesa en edición restablecidas.");
            _logger.Trace("Método CancelarEdicion completado.");
        }


        private void IniciarTimer()
        {
            _logger.Trace("Método IniciarTimer llamado.");
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMinutes(1);
            _timer.Tick += VerificarReservas;
            _timer.Start();
            _logger.Info("Timer para verificar reservas iniciado con intervalo de 1 minuto.");
        }

        private void VerificarReservas(object sender, EventArgs e)
        {
            _logger.Trace("Método VerificarReservas llamado por el timer.");
            DateTime ahora = DateTime.Now;
            _logger.Debug($"Verificando reservas activas a las: {ahora}.");

            try
            {
                var reservasActivas = _context.Reservas
                    .Include(r => r.Mesa)
                    .Where(r => r.FechaHoraFin > ahora)
                    .ToList();
                _logger.Debug($"Se encontraron {reservasActivas.Count} reservas activas.");

                // Usar la colección de reservas del ReservasViewModel
                foreach (var reserva in reservasActivas)
                {
                    _logger.Debug($"Verificando reserva ID: {reserva.Id}, Mesa ID: {reserva.MesaId}, Inicio: {reserva.FechaHoraInicio}, Fin: {reserva.FechaHoraFin}, Estado actual de la mesa: {reserva.Mesa?.Estado}.");
                    if (reserva.FechaHoraInicio <= ahora && reserva.FechaHoraFin >= ahora && reserva.Mesa.Estado != EstadoMesa.Ocupada)
                    {
                        _logger.Info($"Mesa ID: {reserva.MesaId} marcada como Ocupada debido a la reserva ID: {reserva.Id}.");
                        ActualizarEstadoMesa(reserva.MesaId, EstadoMesa.Ocupada);
                    }
                    else if (reserva.FechaHoraInicio.AddMinutes(-10) <= ahora && reserva.FechaHoraInicio > ahora && reserva.Mesa.Estado != EstadoMesa.Pronto)
                    {
                        _logger.Info($"Mesa ID: {reserva.MesaId} marcada como Pronto debido a la reserva ID: {reserva.Id}.");
                        ActualizarEstadoMesa(reserva.MesaId, EstadoMesa.Pronto);
                    }
                    else if (reserva.FechaHoraInicio > ahora && reserva.Mesa.Estado != EstadoMesa.Reservada)
                    {
                        _logger.Info($"Mesa ID: {reserva.MesaId} marcada como Reservada debido a la reserva ID: {reserva.Id}.");
                        ActualizarEstadoMesa(reserva.MesaId, EstadoMesa.Reservada);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al verificar las reservas.");
            }
            _logger.Trace("Método VerificarReservas completado.");
        }

        public void ActualizarEstadoMesa(int mesaId, EstadoMesa nuevoEstado)
        {
            _logger.Trace($"Método ActualizarEstadoMesa llamado para la mesa ID: {mesaId} con el nuevo estado: {nuevoEstado}.");
            var mesa = _context.Mesas.Find(mesaId);
            if (mesa != null)
            {
                _logger.Debug($"Estado anterior de la mesa ID: {mesaId}: {mesa.Estado}.");
                mesa.Estado = nuevoEstado;
                _context.SaveChanges();
                _logger.Info($"Estado de la mesa ID: {mesaId} actualizado a: {nuevoEstado}.");
                CargarMesas();
                OnPropertyChanged(nameof(Mesas));
            }
            else
            {
                _logger.Warn($"No se encontró la mesa con ID: {mesaId} para actualizar su estado.");
            }
            _logger.Trace("Método ActualizarEstadoMesa completado.");
        }
    }
}