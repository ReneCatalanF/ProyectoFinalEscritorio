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
    public class MesaPedidoViewModel : INotifyPropertyChanged
    {
        private readonly RestauranteContext _context;
        public int cantidad = 1;
        public bool guardarPedido { get; set; }

        private string _mensajeError;
        public string MensajeError
        {
            get => _mensajeError;
            set
            {
                _mensajeError = value;
                OnPropertyChanged(nameof(MensajeError));
            }
        }

        private bool _isModalAbierto;
        public bool IsModalAbierto
        {
            get => _isModalAbierto;
            set
            {
                _isModalAbierto = value;
                OnPropertyChanged(nameof(IsModalAbierto));
                if (!value)
                {
                    SelectedMenu = null;
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

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

        private ObservableCollection<Menu> _menus;
        public ObservableCollection<Menu> Menus
        {
            get => _menus;
            set
            {
                if (_menus != null)
                    _menus.CollectionChanged -= Menus_CollectionChanged;

                _menus = value;
                OnPropertyChanged(nameof(Mesas));

                if (_menus != null)
                    _menus.CollectionChanged += Menus_CollectionChanged;
            }
        }

        private Mesa _selectedMesa;
        public Mesa SelectedMesa
        {
            get => _selectedMesa;
            set
            {
                _selectedMesa = value;
                OnPropertyChanged(nameof(SelectedMesa));
                //_eliminarMenuCommand?.RaiseCanExecuteChanged();

                if (SelectedMesa != null)
                {
                    CargarPedidos();
                    //guardarPedido = true;
                }
                else
                {
                    guardarPedido = false;
                }
            }
        }

        private Menu _selectedMenu;
        public Menu SelectedMenu {
            get => _selectedMenu;
            set
            {
                _selectedMenu = value;
                OnPropertyChanged(nameof(SelectedMenu));
                _agregarPedidoCommand?.RaiseCanExecuteChanged();

                if (SelectedMenu != null && SelectedMesa != null)
                {
                    //CargarPedidos();
                    guardarPedido = true;
                }
                else
                {
                    guardarPedido = false;
                }
            }
        }


        private Pedido _newpedido = new Pedido();
        public Pedido NuevoPedido
        {
            get => _newpedido;
            set
            {
                _newpedido = value;
                OnPropertyChanged(nameof(NuevoPedido));
                //_agregarMenuCommand?.RaiseCanExecuteChanged();
            }
        }


        private ObservableCollection<Pedido> _pedidos;
        public ObservableCollection<Pedido> Pedidos {
            get => _pedidos;
            set
            {
                if (_pedidos != null)
                    _pedidos.CollectionChanged -= Pedidos_CollectionChanged;

                _pedidos = value;
                OnPropertyChanged(nameof(Pedidos));

                if (_pedidos != null)
                    _pedidos.CollectionChanged += Pedidos_CollectionChanged;
            }
        }

        private RelayCommand _agregarPedidoCommand;
        public ICommand AgregarPedidoCommand => _agregarPedidoCommand ??= new RelayCommand(AgregarPedido, CanAgregarPedido);

        private RelayCommand _cancelarPedidoCommand;
        public ICommand CancelarPedidoCommand => _cancelarPedidoCommand ??= new RelayCommand(CancelarPedido);

        private RelayCommand _abrirModalCommand;
        public ICommand AbrirModalPedidoCommand => _abrirModalCommand ??= new RelayCommand(AbrirModalPedido);

        private void AbrirModalPedido(object parameter) {
            IsModalAbierto = SelectedMesa != null ? true : false ;
        }

        private void CancelarPedido(object parameter)
        {
            SelectedMenu = null;
            IsModalAbierto = false;
        }

        private void CargarPedidos()
        {
            if (Pedidos != null) {
                Pedidos.Clear();
            }
            
            if (SelectedMesa != null)
            {
                var pedidosDeMesa = _context.Pedidos
                    .Where(p => p.MesaId == SelectedMesa.Id && p.Estado != EstadoPedido.Pagado)
                    .Include(p => p.Menu) // Asegurar que se cargue el Menu relacionado
                    .ToList();

                foreach (var pedido in pedidosDeMesa)
                {
                    Pedidos.Add(pedido);
                }
                OnPropertyChanged(nameof(Pedidos));
            }
        }

        private void Menus_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Menus));
        }

        private void Mesas_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Mesas));
        }

        private void Pedidos_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Pedidos));
        }

        public MesaPedidoViewModel(RestauranteContext context, MesasViewModel mesasViewModel, MenusViewModel menusViewModel) {
            _context = context;
            Pedidos = new ObservableCollection<Pedido>(); ;

            Mesas = mesasViewModel.Mesas;
            mesasViewModel.PropertyChanged += MesasViewModel_PropertyChanged;

            Menus = menusViewModel.Menus;
            menusViewModel.PropertyChanged += MenusViewModel_PropertyChanged;



        }

        private void MesasViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MesasViewModel.Mesas))
            {
                OnPropertyChanged(nameof(Mesas));
            }
        }

        private void MenusViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MenusViewModel.Menus))
            {
                OnPropertyChanged(nameof(Menus));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
        private void AgregarPedido(object parameter)
        {
            if (SelectedMesa == null || SelectedMenu == null) return;

            try
            {
                var nuevoPedido = new Pedido // Crear una nueva instancia aquí
                {
                    MesaId = SelectedMesa.Id,
                    MenuId = SelectedMenu.Id,
                    FechaHora = DateTime.Now,
                    Estado = EstadoPedido.Solicitado,
                    Precio = (SelectedMenu.Precio * cantidad),
                    Cantidad = cantidad
                };

                _context.Pedidos.Add(nuevoPedido);
                _context.SaveChanges();
                IsModalAbierto = false;
                cantidad = 1;
                CargarPedidos(); // Recargar los pedidos después de agregar
                SelectedMenu = null; // Limpiar la selección del menú
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al agregar menu: {ex.Message}";
            }
        }

        private bool CanAgregarPedido(object parameter)
        {
            Console.WriteLine("MESA SELECCIONADA: " + SelectedMesa?.Descripcion +
                " MENU SELECCIONADO: " + SelectedMenu?.Nombre + " CANTIDAD IGUAL A: " + cantidad.ToString());
            return SelectedMesa != null && SelectedMenu != null && cantidad != 0;
        }

    }
}
