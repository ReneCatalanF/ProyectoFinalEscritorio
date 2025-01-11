using Restaurant_SAP.Commands;
using Restaurant_SAP.DB;
using Restaurant_SAP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Restaurant_SAP.ViewModels
{
    
    
    public class MesasViewModel : INotifyPropertyChanged
    {
        private readonly RestauranteContext _context;
        private Mesa _selectedMesa;
        private Mesa _nuevaMesa;
        private string _mensajeError;

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
                // Notificar explicitamente que CanExecute puede haber cambiado
                ((RelayCommand)EliminarMesaCommand).RaiseCanExecuteChanged();
                ((RelayCommand)GuardarCambiosCommand).RaiseCanExecuteChanged();
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
                    _context.Mesas.Remove(SelectedMesa);
                    _context.SaveChanges();

                    Mesas.Remove(SelectedMesa);
                    SelectedMesa = null; // Importantísimo: Deseleccionar la mesa
                    OnMesasChanged();
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al eliminar mesa: {ex.Message}";
            }
        }

        private void GuardarCambios()
        {
            try
            {
                _context.SaveChanges();
                OnPropertyChanged(nameof(Mesas));
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al guardar cambios: {ex.Message}";
            }
        }

        private void CancelarEdicion()
        {
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
    

    
}
