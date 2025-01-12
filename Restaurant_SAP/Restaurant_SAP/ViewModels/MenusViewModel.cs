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
    public class MenusViewModel : INotifyPropertyChanged
    {
        private readonly RestauranteContext _context;
        private Menu _selectedMenu;
        private Menu _menuEnEdicion;
        private string _mensajeError;
        private ObservableCollection<Menu> _menus;
        private Menu _nuevoMenu = new Menu();

        public bool IsEditing { get; set; }

        //public ObservableCollection<EstadoMesa> EstadosMesa { get; } = new ObservableCollection<EstadoMesa>(Enum.GetValues(typeof(EstadoMesa)).Cast<EstadoMesa>());

        public ObservableCollection<Menu> Menus
        {
            get => _menus;
            set
            {
                if (_menus != null)
                    _menus.CollectionChanged -= Menus_CollectionChanged;

                _menus = value;
                OnPropertyChanged(nameof(Menus));

                if (_menus != null)
                    _menus.CollectionChanged += Menus_CollectionChanged;
            }
        }
        private void Menus_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Menus));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Menu SelectedMenu
        {
            get => _selectedMenu;
            set
            {
                _selectedMenu = value;
                OnPropertyChanged(nameof(SelectedMenu));
                _eliminarMenuCommand?.RaiseCanExecuteChanged();

                if (SelectedMenu != null)
                {
                    // Crear una NUEVA instancia de Mesa para la edición
                    MenuEnEdicion = new Menu
                    {
                        Id = SelectedMenu.Id,
                        Nombre = SelectedMenu.Nombre,
                        Descripcion = SelectedMenu.Descripcion,
                        Precio = SelectedMenu.Precio,
                    };
                    IsEditing = true;
                }
                else
                {
                    MenuEnEdicion = null;
                    IsEditing = false;
                }
            }
        }

        public Menu MenuEnEdicion
        {
            get => _menuEnEdicion;
            set
            {
                _menuEnEdicion = value;
                OnPropertyChanged(nameof(MenuEnEdicion));
                _guardarCambiosCommand?.RaiseCanExecuteChanged();
            }
        }

        // Campos privados para los comandos
        private RelayCommand _agregarMenuCommand;
        private RelayCommand _eliminarMenuCommand;
        private RelayCommand _guardarCambiosCommand;
        private RelayCommand _cancelarEdicionCommand;

        // Propiedades públicas que devuelven los comandos (inicialización perezosa)
        public ICommand AgregarMenuCommand => _agregarMenuCommand ??= new RelayCommand(AgregarMenu, CanAgregarMenu);
        public ICommand EliminarMenuCommand => _eliminarMenuCommand ??= new RelayCommand(EliminarMenu, CanEliminarMenu);
        public ICommand GuardarCambiosCommand => _guardarCambiosCommand ??= new RelayCommand(GuardarCambios, CanGuardarCambios);
        public ICommand CancelarEdicionCommand => _cancelarEdicionCommand ??= new RelayCommand(CancelarEdicion);


        public Menu NuevoMenu
        {
            get => _nuevoMenu;
            set
            {
                _nuevoMenu = value;
                OnPropertyChanged(nameof(NuevoMenu));
                _agregarMenuCommand?.RaiseCanExecuteChanged();
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

        public MenusViewModel(RestauranteContext context)
        {
            _context = context;
            Menus = new ObservableCollection<Menu>();

            CargarMenus();

        }

        private void CargarMenus()
        {
            Menus.Clear();
            var menusDesdeBD = _context.Menus.ToList();
            foreach (var menu in menusDesdeBD)
            {
                Menus.Add(menu);
            }
        }

        private void AgregarMenu(object parameter)
        {
            try
            {
                if (string.IsNullOrEmpty(NuevoMenu.Nombre))
                {
                    MensajeError = "El nombre no puede estar vacío.";
                    return;
                }
                if (string.IsNullOrEmpty(NuevoMenu.Descripcion))
                {
                    MensajeError = "La Descripción no puede estar vacía.";
                    return;
                }

                if (NuevoMenu.Precio <= 0)
                {
                    MensajeError = "El precio del menu debe ser mayor que cero.";
                    return;
                }

                if (_context.Menus.Any(m => m.Nombre == NuevoMenu.Nombre))
                {
                    MensajeError = "Ya existe un menu con ese nombre.";
                    return;
                }

                _context.Menus.Add(NuevoMenu);
                _context.SaveChanges();
                CargarMenus();
                NuevoMenu = new Menu();
                MensajeError = "";
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al agregar menu: {ex.Message}";
            }
        }

        private void EliminarMenu(object parameter)
        {
            try
            {
                if (SelectedMenu != null)
                {
                    // Encuentra la menu en el contexto usando su Id
                    var menuAEliminar = _context.Menus.Find(SelectedMenu.Id);

                    if (menuAEliminar != null) // Verifica si la menu existe en la base de datos
                    {
                        _context.Menus.Remove(menuAEliminar);
                        _context.SaveChanges();
                        CargarMenus(); // Recargar las mesas desde la base de datos
                        SelectedMenu = null; // Limpiar la selección
                        MenuEnEdicion = null;
                        MensajeError = "";
                    }
                    else
                    {
                        MensajeError = "El menu ya no existe en la base de datos.";
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                MensajeError = $"Error al eliminar el menu (problema de dependencias): {ex.Message}";
                // Puedes agregar un manejo más específico de la excepción, como mostrar un mensaje diferente al usuario.
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al eliminar el menu: {ex.Message}";
            }
        }

        private bool CanEliminarMenu(object parameter)
        {
            return SelectedMenu != null;
        }

        private void GuardarCambios(object parameter)
        {
            try
            {
                if (MenuEnEdicion != null)
                {
                    var menuOriginal = _context.Menus.Find(SelectedMenu.Id);

                    if (menuOriginal != null)
                    {
                        menuOriginal.Nombre = MenuEnEdicion.Nombre;
                        menuOriginal.Descripcion = MenuEnEdicion.Descripcion;
                        menuOriginal.Precio = MenuEnEdicion.Precio;

                        _context.SaveChanges();
                        CargarMenus();
                        IsEditing = false;
                        SelectedMenu = null;
                        MenuEnEdicion = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al guardar cambios: {ex.Message}";
            }
        }
        private bool CanAgregarMenu(object parameter)
        {
            return true;
        }

        private bool CanGuardarCambios(object parameter)
        {
            return MenuEnEdicion != null &&
                   MenuEnEdicion.Precio > 0 &&
                   !string.IsNullOrEmpty(MenuEnEdicion.Descripcion) &&
                   !string.IsNullOrEmpty(MenuEnEdicion.Nombre) &&
                   !Menus.Any(m => m.Nombre == MenuEnEdicion.Nombre && m.Id != MenuEnEdicion.Id);
        }

        private void CancelarEdicion(object parameter)
        {
            IsEditing = false;
            MenuEnEdicion = null;
            SelectedMenu = null;
        }

    }
}
