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
using NLog;

namespace Restaurant_SAP.ViewModels
{
    public class HistorialEconomicoViewModel : INotifyPropertyChanged
    {
        private readonly RestauranteContext _context;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public event PropertyChangedEventHandler? PropertyChanged;

        private double _total = 0;
        public double Total
        {
            get => _total;
            set
            {
                _total = value;
                OnPropertyChanged(nameof(Total));
                _logger.Debug($"Total económico actualizado: {_total}.");
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
                _logger.Debug($"Colección de pedidos para el historial actualizada. Nuevo conteo: {_pedidos?.Count}.");
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
                _logger.Debug($"Mesa seleccionada para el filtro: {(_mesaSeleccionada as Mesa)?.Descripcion ?? "Todos"}.");
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
                _logger.Debug($"Menú seleccionado para el filtro: {(_menuSeleccionado as Menu)?.Nombre ?? "Todos"}.");
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
                _logger.Debug($"Fecha de inicio del filtro establecida en: {_fechaDesde?.ToShortDateString() ?? "Sin especificar"}.");
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
                _logger.Debug($"Fecha de fin del filtro establecida en: {_fechaHasta?.ToShortDateString() ?? "Sin especificar"}.");
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
                _logger.Debug($"Colección de menús cargada para el filtro. Conteo: {_menus?.Count}.");
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
                _logger.Debug($"Colección de mesas cargada para el filtro. Conteo: {_mesas?.Count}.");
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
            _logger.Trace("Constructor HistorialEconomicoViewModel llamado.");
            _context = context;
            Pedidos = new ObservableCollection<Pedido>();
            Menus = new ObservableCollection<Menu>();
            Mesas = new ObservableCollection<Mesa>();
            BuscarPedidosCommand = new RelayCommand(BuscarPedidos);
            GenerarPdfCommand = new RelayCommand(GenerarPdf); // Inicializar el nuevo comando
            CargarMenusYMesas();
            _logger.Debug($"HistorialEconomicoViewModel inicializado con contexto: {_context}.");
        }

        public void CargarMenusYMesas()
        {
            _logger.Trace("Cargando menús y mesas para los filtros.");
            // Cargar Menús
            Menus.Clear();
            var menusDesdeDb = _context.Menus.ToList();
            Menus.Add(new Menu { Id = 0, Nombre = "Todos" });
            foreach (var menu in menusDesdeDb)
            {
                Menus.Add(menu);
            }
            OnPropertyChanged(nameof(Menus));
            _logger.Info($"Se cargaron {menusDesdeDb.Count} menús para el filtro.");

            // Cargar Mesas
            Mesas.Clear();
            var mesasDesdeDb = _context.Mesas.ToList();
            Mesas.Add(new Mesa { Id = 0, Numero = 0 });
            foreach (var mesa in mesasDesdeDb)
            {
                Mesas.Add(mesa);
            }
            OnPropertyChanged(nameof(Mesas));
            _logger.Info($"Se cargaron {mesasDesdeDb.Count} mesas para el filtro.");

            // Limpiar los pedidos al cargar la vista
            Pedidos.Clear();
            Total = 0;
            OnPropertyChanged(nameof(Pedidos));
            OnPropertyChanged(nameof(Total));
            _logger.Debug("Pedidos y total inicializados/limpiados.");

            // Establecer "Todos" como la selección inicial
            MesaSeleccionada = Mesas.First();
            MenuSeleccionado = Menus.First();
            _logger.Debug("Filtros de mesa y menú establecidos en 'Todos' inicialmente.");
            _logger.Trace("Carga de menús y mesas completada.");
        }

        public void BuscarPedidos(object parameter)
        {
            _logger.Trace("Método BuscarPedidos llamado.");
            var filtroPedidos = _context.Pedidos.Where(p => p.Estado == EstadoPedido.Pagado);
            _logger.Debug("Iniciando búsqueda de pedidos pagados.");

            if (MesaSeleccionada != null && (MesaSeleccionada as Mesa)?.Id != 0)
            {
                filtroPedidos = filtroPedidos.Where(p => p.MesaId == (MesaSeleccionada as Mesa).Id);
                _logger.Debug($"Filtrando por Mesa ID: {(MesaSeleccionada as Mesa).Id}.");
            }

            if (MenuSeleccionado != null && (MenuSeleccionado as Menu)?.Id != 0)
            {
                filtroPedidos = filtroPedidos.Where(p => p.MenuId == (MenuSeleccionado as Menu).Id);
                _logger.Debug($"Filtrando por Menú ID: {(MenuSeleccionado as Menu).Id}.");
            }

            if (FechaDesde.HasValue)
            {
                filtroPedidos = filtroPedidos.Where(p => p.FechaHora >= FechaDesde.Value.Date);
                _logger.Debug($"Filtrando desde la fecha: {FechaDesde.Value.ToShortDateString()}.");
            }

            if (FechaHasta.HasValue)
            {
                filtroPedidos = filtroPedidos.Where(p => p.FechaHora <= FechaHasta.Value.Date.AddDays(1).AddSeconds(-1));
                _logger.Debug($"Filtrando hasta la fecha: {FechaHasta.Value.ToShortDateString()}.");
            }

            Pedidos = new ObservableCollection<Pedido>(filtroPedidos.ToList());
            Total = Pedidos.Sum(pedido => pedido.Precio);
            OnPropertyChanged(nameof(Pedidos));
            OnPropertyChanged(nameof(Total));
            _logger.Info($"Se encontraron {Pedidos.Count} pedidos con los criterios de búsqueda. Total: {Total}.");
            _logger.Trace("Método BuscarPedidos completado.");
        }

        private void GenerarPdf(object parameter)
        {
            _logger.Trace("Método GenerarPdf llamado.");
            if (Pedidos != null && Pedidos.Any())
            {
                _logger.Debug($"Generando PDF para {Pedidos.Count} pedidos.");
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
                        _logger.Info($"PDF guardado exitosamente en: {dialog.FileName}.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al guardar el PDF: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        _logger.Error(ex, "Error al guardar el archivo PDF.");
                    }
                }
                else
                {
                    _logger.Debug("Operación de guardar PDF cancelada por el usuario.");
                }
            }
            else
            {
                MessageBox.Show("No hay pedidos para generar el PDF.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                _logger.Info("No hay pedidos para generar el PDF.");
            }
            _logger.Trace("Método GenerarPdf completado.");
        }
    }
}