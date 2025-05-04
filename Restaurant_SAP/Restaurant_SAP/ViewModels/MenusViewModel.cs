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
    public class MenusViewModel : INotifyPropertyChanged
    {
        private readonly RestauranteContext _context;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private Menu _selectedMenu;
        private Menu _menuEnEdicion;
        private string _mensajeError;
        private ObservableCollection<Menu> _menus;
        private Menu _nuevoMenu = new Menu();

        public bool IsEditing { get; set; }

        public ObservableCollection<Menu> Menus
        {
            get => _menus;
            set
            {
                if (_menus != null)
                    _menus.CollectionChanged -= Menus_CollectionChanged;

                _menus = value;
                OnPropertyChanged(nameof(Menus));
                _logger.Debug($"La colección de menús ha sido actualizada. Nuevo conteo: {_menus?.Count}");

                if (_menus != null)
                    _menus.CollectionChanged += Menus_CollectionChanged;
            }
        }
        private void Menus_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Menus));
            _logger.Debug($"La colección de menús ha cambiado. Acción: {e.Action}.");
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
                _logger.Debug($"Menú seleccionado cambiado. Nuevo menú: {_selectedMenu?.Nombre} (ID: {_selectedMenu?.Id}).");
                _eliminarMenuCommand?.RaiseCanExecuteChanged();

                if (SelectedMenu != null)
                {
                    MenuEnEdicion = new Menu
                    {
                        Id = SelectedMenu.Id,
                        Nombre = SelectedMenu.Nombre,
                        Descripcion = SelectedMenu.Descripcion,
                        Precio = SelectedMenu.Precio,
                    };
                    IsEditing = true;
                    _logger.Debug($"Iniciando edición del menú ID: {SelectedMenu.Id}.");
                }
                else
                {
                    MenuEnEdicion = null;
                    IsEditing = false;
                    _logger.Debug("Edición del menú cancelada o no se seleccionó ningún menú.");
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
                _logger.Debug($"Menú en edición cambiado. Nuevo menú en edición: {_menuEnEdicion?.Nombre} (ID: {_menuEnEdicion?.Id}).");
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
                _logger.Debug($"Nuevo menú creado o modificado: Nombre: {_nuevoMenu?.Nombre}, Descripción: {_nuevoMenu?.Descripcion}, Precio: {_nuevoMenu?.Precio}.");
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
                if (!string.IsNullOrEmpty(_mensajeError))
                {
                    _logger.Error($"Mensaje de error establecido: {_mensajeError}");
                }
            }
        }

        public MenusViewModel(RestauranteContext context)
        {
            _logger.Trace("Constructor MenusViewModel llamado.");
            _context = context;
            Menus = new ObservableCollection<Menu>();
            CargarMenus();
            _logger.Debug($"MenusViewModel inicializado con contexto: {_context}.");
        }

        private void CargarMenus()
        {
            Menus.Clear();
            _logger.Trace("Cargando menús desde la base de datos...");
            var menusDesdeBD = _context.Menus.ToList();
            foreach (var menu in menusDesdeBD)
            {
                Menus.Add(menu);
            }
            _logger.Info($"Se cargaron {menusDesdeBD.Count} menús desde la base de datos.");
        }

        private void AgregarMenu(object parameter)
        {
            _logger.Trace("Método AgregarMenu llamado.");
            try
            {
                if (string.IsNullOrEmpty(NuevoMenu.Nombre))
                {
                    MensajeError = "El nombre no puede estar vacío.";
                    _logger.Warn("Intento de agregar menú con nombre vacío.");
                    return;
                }
                if (string.IsNullOrEmpty(NuevoMenu.Descripcion))
                {
                    MensajeError = "La descripción no puede estar vacía.";
                    _logger.Warn("Intento de agregar menú con descripción vacía.");
                    return;
                }
                if (NuevoMenu.Precio <= 0)
                {
                    MensajeError = "El precio del menú debe ser mayor que cero.";
                    _logger.Warn($"Intento de agregar menú con precio no válido: {NuevoMenu.Precio}.");
                    return;
                }
                if (_context.Menus.Any(m => m.Nombre == NuevoMenu.Nombre))
                {
                    MensajeError = "Ya existe un menú con ese nombre.";
                    _logger.Warn($"Intento de agregar menú con nombre duplicado: {NuevoMenu.Nombre}.");
                    return;
                }

                _context.Menus.Add(NuevoMenu);
                _context.SaveChanges();
                _logger.Info($"Menú agregado. Nombre: {NuevoMenu.Nombre}, Descripción: {NuevoMenu.Descripcion}, Precio: {NuevoMenu.Precio}.");
                CargarMenus();
                NuevoMenu = new Menu();
                MensajeError = "";
                _logger.Debug("NuevoMenu restablecido.");
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al agregar menú: {ex.Message}";
                _logger.Error(ex, "Error al agregar menú.");
            }
            _logger.Trace("Método AgregarMenu completado.");
        }

        private void EliminarMenu(object parameter)
        {
            _logger.Trace("Método EliminarMenu llamado.");
            try
            {
                if (SelectedMenu != null)
                {
                    var menuAEliminar = _context.Menus.Find(SelectedMenu.Id);
                    if (menuAEliminar != null)
                    {
                        _context.Menus.Remove(menuAEliminar);
                        _context.SaveChanges();
                        _logger.Info($"Menú eliminado. ID: {menuAEliminar.Id}, Nombre: {menuAEliminar.Nombre}.");
                        CargarMenus();
                        SelectedMenu = null;
                        MenuEnEdicion = null;
                        MensajeError = "";
                        _logger.Debug("Selección y edición de menú restablecidas después de la eliminación.");
                    }
                    else
                    {
                        MensajeError = "El menú ya no existe en la base de datos.";
                        _logger.Warn($"Intento de eliminar un menú que ya no existe en la base de datos. ID: {SelectedMenu.Id}.");
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                MensajeError = $"Error al eliminar el menú (problema de dependencias): {ex.Message}";
                _logger.Error(ex, $"Error al eliminar el menú ID: {SelectedMenu?.Id} debido a dependencias.");
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al eliminar el menú: {ex.Message}";
                _logger.Error(ex, $"Error inesperado al eliminar el menú ID: {SelectedMenu?.Id}.");
            }
            _logger.Trace("Método EliminarMenu completado.");
        }

        private bool CanEliminarMenu(object parameter)
        {
            bool canExecute = SelectedMenu != null;
            _logger.Trace($"CanEliminarMenu llamado. Retornando: {canExecute}.");
            return canExecute;
        }

        private void GuardarCambios(object parameter)
        {
            _logger.Trace("Método GuardarCambios llamado.");
            try
            {
                if (MenuEnEdicion != null)
                {
                    var menuOriginal = _context.Menus.Find(SelectedMenu.Id);
                    if (menuOriginal != null)
                    {
                        _logger.Debug($"Guardando cambios en el menú ID: {SelectedMenu.Id}. Nombre anterior: {menuOriginal.Nombre}, Nuevo nombre: {MenuEnEdicion.Nombre}. Descripción anterior: {menuOriginal.Descripcion}, Nueva descripción: {MenuEnEdicion.Descripcion}. Precio anterior: {menuOriginal.Precio}, Nuevo precio: {MenuEnEdicion.Precio}.");
                        menuOriginal.Nombre = MenuEnEdicion.Nombre;
                        menuOriginal.Descripcion = MenuEnEdicion.Descripcion;
                        menuOriginal.Precio = MenuEnEdicion.Precio;

                        _context.SaveChanges();
                        _logger.Info($"Cambios guardados en el menú ID: {menuOriginal.Id}.");
                        CargarMenus();
                        IsEditing = false;
                        SelectedMenu = null;
                        MenuEnEdicion = null;
                        _logger.Debug("Edición completada. Selección y menú en edición restablecidas.");
                    }
                    else
                    {
                        _logger.Warn($"No se encontró el menú con ID: {SelectedMenu.Id} para guardar los cambios.");
                    }
                }
                else
                {
                    _logger.Warn("No se pueden guardar los cambios: MenuEnEdicion es null.");
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al guardar cambios: {ex.Message}";
                _logger.Error(ex, $"Error al guardar cambios en el menú ID: {SelectedMenu?.Id}.");
            }
            _logger.Trace("Método GuardarCambios completado.");
        }
        private bool CanAgregarMenu(object parameter)
        {
            _logger.Trace($"CanAgregarMenu llamado. Retornando: true.");
            return true;
        }

        private bool CanGuardarCambios(object parameter)
        {
            bool canExecute = MenuEnEdicion != null &&
                               MenuEnEdicion.Precio > 0 &&
                               !string.IsNullOrEmpty(MenuEnEdicion.Descripcion) &&
                               !string.IsNullOrEmpty(MenuEnEdicion.Nombre) &&
                               !Menus.Any(m => m.Nombre == MenuEnEdicion.Nombre && m.Id != MenuEnEdicion.Id);
            _logger.Trace($"CanGuardarCambios llamado. Retornando: {canExecute}.");
            return canExecute;
        }

        private void CancelarEdicion(object parameter)
        {
            _logger.Trace("Método CancelarEdicion llamado.");
            IsEditing = false;
            MenuEnEdicion = null;
            SelectedMenu = null;
            _logger.Debug("Edición cancelada. Selección y menú en edición restablecidas.");
            _logger.Trace("Método CancelarEdicion completado.");
        }
    }
}