using Restaurant_SAP.Commands; // Asegúrate de que esta using esté correcta
using Restaurant_SAP.DB;
using Restaurant_SAP.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Restaurant_SAP.ViewModels
{
    public class HistorialEconomicoViewModel : INotifyPropertyChanged
    {
        private readonly RestauranteContext _context;
        public event PropertyChangedEventHandler? PropertyChanged;

        private double _total = 0;
        public double Total
        {
            get => _total;
            set
            {
                _total = value;
                OnPropertyChanged(nameof(Total));
            }
        }

        private ObservableCollection<Pedido> _pedidos;
        public ObservableCollection<Pedido> Pedidos
        {
            get => _pedidos;
            set
            {
                _pedidos = value;
                OnPropertyChanged(nameof(Pedidos));
            }
        }

        private string _filtroMesa;
        public string FiltroMesa
        {
            get => _filtroMesa;
            set
            {
                _filtroMesa = value;
                OnPropertyChanged(nameof(FiltroMesa));
            }
        }

        private Menu _menuSeleccionado;
        public Menu MenuSeleccionado
        {
            get => _menuSeleccionado;
            set
            {
                _menuSeleccionado = value;
                OnPropertyChanged(nameof(MenuSeleccionado));
            }
        }

        private DateTime? _fechaDesde;
        public DateTime? FechaDesde
        {
            get => _fechaDesde;
            set
            {
                _fechaDesde = value;
                OnPropertyChanged(nameof(FechaDesde));
            }
        }

        private DateTime? _fechaHasta;
        public DateTime? FechaHasta
        {
            get => _fechaHasta;
            set
            {
                _fechaHasta = value;
                OnPropertyChanged(nameof(FechaHasta));
            }
        }

        private ObservableCollection<Menu> _menus;
        public ObservableCollection<Menu> Menus
        {
            get => _menus;
            set
            {
                _menus = value;
                OnPropertyChanged(nameof(Menus));
            }
        }

        private ObservableCollection<Mesa> _mesas;
        public ObservableCollection<Mesa> Mesas
        {
            get => _mesas;
            set
            {
                _mesas = value;
                OnPropertyChanged(nameof(Mesas));
            }
        }

        public ICommand BuscarPedidosCommand { get; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public HistorialEconomicoViewModel(RestauranteContext context)
        {
            _context = context;
            Pedidos = new ObservableCollection<Pedido>();
            Menus = new ObservableCollection<Menu>();
            Mesas = new ObservableCollection<Mesa>();
            BuscarPedidosCommand = new RelayCommand(BuscarPedidos); // Usando tu RelayCommand
            CargarMenusYMesas();
        }

        public void CargarMenusYMesas()
        {
            // Cargar Menús
            Menus.Clear();
            var menusDesdeDb = _context.Menus.ToList();
            foreach (var menu in menusDesdeDb)
            {
                Menus.Add(menu);
            }
            OnPropertyChanged(nameof(Menus));

            // Cargar Mesas
            Mesas.Clear();
            var mesasDesdeDb = _context.Mesas.ToList();
            foreach (var mesa in mesasDesdeDb)
            {
                Mesas.Add(mesa);
            }
            OnPropertyChanged(nameof(Mesas));

            // Limpiar los pedidos al cargar la vista
            Pedidos.Clear();
            Total = 0;
            OnPropertyChanged(nameof(Pedidos));
            OnPropertyChanged(nameof(Total));
        }

        public void BuscarPedidos(object parameter) // El Execute de tu RelayCommand espera un objeto
        {
            var filtroPedidos = _context.Pedidos.Where(p => p.Estado == EstadoPedido.Pagado);

            if (!string.IsNullOrEmpty(FiltroMesa))
            {
                if (int.TryParse(FiltroMesa, out int numeroMesa))
                {
                    filtroPedidos = filtroPedidos.Where(p => p.Mesa.Numero == numeroMesa);
                }
                // Si no se puede convertir a entero, no se aplica el filtro de mesa
            }

            if (MenuSeleccionado != null)
            {
                filtroPedidos = filtroPedidos.Where(p => p.MenuId == MenuSeleccionado.Id);
            }

            if (FechaDesde.HasValue)
            {
                filtroPedidos = filtroPedidos.Where(p => p.FechaHora >= FechaDesde.Value.Date);
            }

            if (FechaHasta.HasValue)
            {
                filtroPedidos = filtroPedidos.Where(p => p.FechaHora <= FechaHasta.Value.Date.AddDays(1).AddSeconds(-1));
            }

            Pedidos = new ObservableCollection<Pedido>(filtroPedidos.ToList());
            Total = Pedidos.Sum(pedido => pedido.Precio);
            OnPropertyChanged(nameof(Pedidos));
            OnPropertyChanged(nameof(Total));
        }
    }
}