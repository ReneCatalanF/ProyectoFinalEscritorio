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

        private object _mesaSeleccionada; // Cambiamos el tipo a object para aceptar el "Todos"
        public object MesaSeleccionada
        {
            get => _mesaSeleccionada;
            set
            {
                _mesaSeleccionada = value;
                OnPropertyChanged(nameof(MesaSeleccionada));
            }
        }

        private object _menuSeleccionado; // Cambiamos el tipo a object para aceptar el "Todos"
        public object MenuSeleccionado
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
            Menus.Add(new Menu { Id = 0, Nombre = "Todos" }); // Añadir "Todos" al principio
            foreach (var menu in menusDesdeDb)
            {
                Menus.Add(menu);
            }
            OnPropertyChanged(nameof(Menus));

            // Cargar Mesas
            Mesas.Clear();
            var mesasDesdeDb = _context.Mesas.ToList();
            Mesas.Add(new Mesa { Id = 0, Numero = 0 }); // Añadir "Todos" al principio (usamos 0.1 para evitar conflicto con mesas reales)
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

            // Establecer "Todos" como la selección inicial
            MesaSeleccionada = Mesas.First();
            MenuSeleccionado = Menus.First();
        }

        public void BuscarPedidos(object parameter) // El Execute de tu RelayCommand espera un objeto
        {
            var filtroPedidos = _context.Pedidos.Where(p => p.Estado == EstadoPedido.Pagado);

            if (MesaSeleccionada != null && (MesaSeleccionada as Mesa)?.Id != 0) // Filtrar si no es "Todos"
            {
                filtroPedidos = filtroPedidos.Where(p => p.MesaId == (MesaSeleccionada as Mesa).Id);
            }

            if (MenuSeleccionado != null && (MenuSeleccionado as Menu)?.Id != 0) // Filtrar si no es "Todos"
            {
                filtroPedidos = filtroPedidos.Where(p => p.MenuId == (MenuSeleccionado as Menu).Id);
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