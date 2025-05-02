using Restaurant_SAP.Commands; // Asegúrate de que esta using esté correcta
using Restaurant_SAP.DB;
using Restaurant_SAP.Models;
using Restaurant_SAP.Utilities; // Namespace para PdfGenerator
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32; // Para SaveFileDialog

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
        public ICommand GenerarPdfCommand { get; } // Nuevo comando

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
            BuscarPedidosCommand = new RelayCommand(BuscarPedidos);
            GenerarPdfCommand = new RelayCommand(GenerarPdf); // Inicializar el nuevo comando
            CargarMenusYMesas();
        }

        public void CargarMenusYMesas()
        {
            // Cargar Menús
            Menus.Clear();
            var menusDesdeDb = _context.Menus.ToList();
            Menus.Add(new Menu { Id = 0, Nombre = "Todos" });
            foreach (var menu in menusDesdeDb)
            {
                Menus.Add(menu);
            }
            OnPropertyChanged(nameof(Menus));

            // Cargar Mesas
            Mesas.Clear();
            var mesasDesdeDb = _context.Mesas.ToList();
            Mesas.Add(new Mesa { Id = 0, Numero = 0 });
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

        public void BuscarPedidos(object parameter)
        {
            var filtroPedidos = _context.Pedidos.Where(p => p.Estado == EstadoPedido.Pagado);

            if (MesaSeleccionada != null && (MesaSeleccionada as Mesa)?.Id != 0)
            {
                filtroPedidos = filtroPedidos.Where(p => p.MesaId == (MesaSeleccionada as Mesa).Id);
            }

            if (MenuSeleccionado != null && (MenuSeleccionado as Menu)?.Id != 0)
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

        private void GenerarPdf(object parameter)
        {
            if (Pedidos != null && Pedidos.Any())
            {
                byte[] pdfBytes = PdfGenerator.GeneratePedidosPdf(Pedidos.ToList());

                SaveFileDialog dialog = new SaveFileDialog
                {
                    FileName = "HistorialPedidos.pdf",
                    DefaultExt = ".pdf",
                    Filter = "PDF files (.pdf)|*.pdf"
                };

                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        File.WriteAllBytes(dialog.FileName, pdfBytes);
                        MessageBox.Show($"El PDF se ha guardado en: {dialog.FileName}", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al guardar el PDF: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay pedidos para generar el PDF.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}