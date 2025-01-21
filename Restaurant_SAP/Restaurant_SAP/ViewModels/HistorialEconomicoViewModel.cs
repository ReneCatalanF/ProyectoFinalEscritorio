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

namespace Restaurant_SAP.ViewModels
{
    public class HistorialEconomicoViewModel : INotifyPropertyChanged
    {
        private readonly RestauranteContext _context;
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<Pedido> _pedidos;
        public ObservableCollection<Pedido> Pedidos
        {
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
        private void Pedidos_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Pedidos));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public HistorialEconomicoViewModel(RestauranteContext context, MesaPedidoViewModel mesaPedidoViewModel) {
            _context = context;
            Pedidos = new ObservableCollection<Pedido>();

            mesaPedidoViewModel.PropertyChanged += MesaPedidosViewModel_PropertyChanged;

            CargarPedidos();
            
        }

        private void MesaPedidosViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MesaPedidoViewModel.Pedidos))
            {
                CargarPedidos();
                OnPropertyChanged(nameof(Pedidos));
            }
        }

        private void CargarPedidos()
        {
            if (Pedidos != null)
            {
                Pedidos.Clear();
            }

            var pedidos = _context.Pedidos
                    .Where(p =>  p.Estado == EstadoPedido.Pagado)
                    .ToList();

            foreach (var pedido in pedidos)
            {
                Pedidos.Add(pedido);
            }
            OnPropertyChanged(nameof(Pedidos));
        }
    }
}
