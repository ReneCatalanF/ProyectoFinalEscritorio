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

                if (_mesas != null)
                    _mesas.CollectionChanged += Mesas_CollectionChanged;
            }
        }

        private void Mesas_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Mesas));
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
                _eliminarMesaCommand?.RaiseCanExecuteChanged();

                if (SelectedMesa != null)
                {
                    // Crear una NUEVA instancia de Mesa para la edición
                    MesaEnEdicion = new Mesa
                    {
                        Id = SelectedMesa.Id,
                        Numero = SelectedMesa.Numero,
                        Descripcion = SelectedMesa.Descripcion,
                        Estado = SelectedMesa.Estado 
                    };
                    IsEditing = true;
                }
                else
                {
                    MesaEnEdicion = null;
                    IsEditing = false;
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
            }
        }


        public MesasViewModel(RestauranteContext context)
        {
            _context = context;
            Mesas = new ObservableCollection<Mesa>();
            
            CargarMesas();

        }

        private void CargarMesas()
        {
            Mesas.Clear();
            var mesasDesdeBD = _context.Mesas.ToList();
            foreach (var mesa in mesasDesdeBD)
            {
                Mesas.Add(mesa);
            }
        }

        private void AgregarMesa(object parameter)
        {
            try
            {
                if (string.IsNullOrEmpty(NuevaMesa.Descripcion))
                {
                    MensajeError = "La descripción no puede estar vacía.";
                    return;
                }
                if (NuevaMesa.Numero <= 0)
                {
                    MensajeError = "El número de mesa debe ser mayor que cero.";
                    return;
                }
                if (_context.Mesas.Any(m => m.Numero == NuevaMesa.Numero))
                {
                    MensajeError = "Ya existe una mesa con ese número.";
                    return;
                }

                _context.Mesas.Add(NuevaMesa);
                _context.SaveChanges();
                CargarMesas();
                NuevaMesa = new Mesa();
                MensajeError = "";
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al agregar mesa: {ex.Message}";
            }
        }

        /*
        private void EliminarMesa(object parameter)
        {
            try
            {
                if (SelectedMesa != null)
                {
                    _context.Mesas.Remove(SelectedMesa);
                    _context.SaveChanges();
                    CargarMesas();
                    SelectedMesa = null;
                    MesaEnEdicion = null;
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al eliminar mesa: {ex.Message}";
            }
        }
        */

        private void EliminarMesa(object parameter)
        {
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
                        CargarMesas(); // Recargar las mesas desde la base de datos
                        SelectedMesa = null; // Limpiar la selección
                        MesaEnEdicion = null;
                        MensajeError = "";
                    }
                    else
                    {
                        MensajeError = "La mesa ya no existe en la base de datos.";
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                MensajeError = $"Error al eliminar la mesa (problema de dependencias): {ex.Message}";
                // Puedes agregar un manejo más específico de la excepción, como mostrar un mensaje diferente al usuario.
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al eliminar la mesa: {ex.Message}";
            }
        }

        private bool CanEliminarMesa(object parameter)
        {
            return SelectedMesa != null;
        }

        private void GuardarCambios(object parameter)
        {
            try
            {
                if (MesaEnEdicion != null)
                {
                    var mesaOriginal = _context.Mesas.Find(SelectedMesa.Id);

                    if (mesaOriginal != null)
                    {
                        mesaOriginal.Numero = MesaEnEdicion.Numero;
                        mesaOriginal.Descripcion = MesaEnEdicion.Descripcion;
                        mesaOriginal.Estado = MesaEnEdicion.Estado;

                        _context.SaveChanges();
                        CargarMesas();
                        IsEditing = false;
                        SelectedMesa = null;
                        MesaEnEdicion = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al guardar cambios: {ex.Message}";
            }
        }
        private bool CanAgregarMesa(object parameter)
        {
            return true;
        }

        private bool CanGuardarCambios(object parameter)
        {
            return MesaEnEdicion != null &&
                   MesaEnEdicion.Numero > 0 &&
                   !string.IsNullOrEmpty(MesaEnEdicion.Descripcion) &&
                   !Mesas.Any(m => m.Numero == MesaEnEdicion.Numero && m.Id != MesaEnEdicion.Id);
        }

        private void CancelarEdicion(object parameter)
        {
            IsEditing = false;
            MesaEnEdicion = null;
            SelectedMesa = null;
        }
    }
    


    
    


}

