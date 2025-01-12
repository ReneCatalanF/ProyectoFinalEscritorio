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

    /*
    public class MesasViewModel : INotifyPropertyChanged
    {
        private readonly RestauranteContext _context;
        private Mesa _selectedMesa;
        private Mesa _nuevaMesa;
        private Mesa _mesaEnEdicion;
        private string _mensajeError;

        public bool IsEditing { get; set; }

        public ObservableCollection<Mesa> Mesas { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;



        public static event Action MesasChanged;

        
        public Mesa SelectedMesa
        {
            get => _selectedMesa;
            set
            {
                _selectedMesa = value;
                OnPropertyChanged(nameof(SelectedMesa));
                IsEditing = value != null;
                // Notificar explicitamente que CanExecute puede haber cambiado
                ((RelayCommand)EliminarMesaCommand).RaiseCanExecuteChanged();
                ((RelayCommand)GuardarCambiosCommand).RaiseCanExecuteChanged();
            }
        }
        

        public Mesa SelectedMesa
        {
            get => _selectedMesa;
            set
            {
                _selectedMesa = value;
                OnPropertyChanged(nameof(SelectedMesa));

                if (SelectedMesa != null)
                {
                    // Crear una copia de la mesa seleccionada para la edición
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
            }
        }


        public Mesa NuevaMesa
        {
            get => _nuevaMesa;
            set
            {
                _nuevaMesa = value;
                OnPropertyChanged(nameof(NuevaMesa));
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

        public ICommand AgregarMesaCommand { get; }
        public ICommand EliminarMesaCommand { get; }
        public ICommand GuardarCambiosCommand { get; }
        public ICommand CancelarEdicionCommand { get; }


        public MesasViewModel()
        {
            _context = new RestauranteContext();
            CargarMesas();
            NuevaMesa = new Mesa();



            AgregarMesaCommand = new RelayCommand(AgregarMesa);
            EliminarMesaCommand = new RelayCommand(EliminarMesa);
            GuardarCambiosCommand = new RelayCommand(GuardarCambios);
            CancelarEdicionCommand = new RelayCommand(CancelarEdicion);

        }

        private void CargarMesas()
        {
            Mesas = new ObservableCollection<Mesa>(_context.Mesas.ToList());
        }

        private void AgregarMesa()
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

                _context.Mesas.Add(NuevaMesa); // Solo agregas la nueva mesa con la descripción
                _context.SaveChanges(); // Aquí se genera el Id
                Mesas.Add(NuevaMesa);
                CargarMesas();
                NuevaMesa = new Mesa(); // Limpiar el formulario
                MensajeError = "";

                OnMesasChanged();
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al agregar mesa: {ex.Message}";
            }
        }




        private void EliminarMesa()
        {
            try
            {

                if (SelectedMesa != null)
                {
                    MesaEnEdicion = null;
                    IsEditing = false;
                    _context.Mesas.Remove(SelectedMesa);
                    _context.SaveChanges();

                    Mesas.Remove(SelectedMesa);
                    //CargarMesas();
                    SelectedMesa = null; // Importantísimo: Deseleccionar la mesa
                    OnMesasChanged();
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al eliminar mesa: {ex.Message}";
            }
        }

        private bool EsNumeroDeMesaValido(Mesa mesaAValidar)
        {
            if (mesaAValidar == null)
            {
                MensajeError = "No se puede validar una mesa nula.";
                return false;
            }

            if (mesaAValidar.Numero <= 0)
            {
                MensajeError = "El número de mesa debe ser mayor que cero.";
                return false;
            }

            // Validación para evitar números de mesa repetidos (EXCEPTO para la misma mesa)
            if (_context.Mesas.Any(m => m.Numero == mesaAValidar.Numero && m.Id != mesaAValidar.Id))
            {
                MensajeError = "Ya existe una mesa con ese número.";
                return false;
            }

            MensajeError = ""; // Limpiar el mensaje de error si la validación es exitosa
            return true;
        }

        private void GuardarCambios()
        {
            try
            {
                if (MesaEnEdicion == null) return;

                if (!EsNumeroDeMesaValido(MesaEnEdicion)) return;

                //Copiar los valores del objeto temporal al objeto original
                SelectedMesa.Numero = MesaEnEdicion.Numero;
                SelectedMesa.Descripcion = MesaEnEdicion.Descripcion;

                _context.Entry(SelectedMesa).State = EntityState.Modified;
                _context.SaveChanges();

                CargarMesas();
                OnPropertyChanged(nameof(Mesas));
                //CargarMesas();
                IsEditing = false;
                MensajeError = "";
                MesaEnEdicion = null;
                SelectedMesa = null;
                OnMesasChanged();
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al guardar cambios: {ex.Message}";
            }
        }


        private void CancelarEdicion()
        {
            IsEditing = false;
            NuevaMesa = new Mesa();
            MesaEnEdicion = null;
            //Recargar las mesas desde la base de datos para cancelar los cambios
            _context.ChangeTracker.Clear();
            CargarMesas();
            SelectedMesa = null;
        }
        

        protected virtual void OnMesasChanged()
        {
            MesasChanged?.Invoke();
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
    */

    
    
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

