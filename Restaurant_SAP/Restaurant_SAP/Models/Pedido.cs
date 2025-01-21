using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_SAP.Models
{
    public enum EstadoPedido { Solicitado, Servido, Pagado }
    public class Pedido
    {
        public int Id { get; set; }
        public int MesaId { get; set; }
        public virtual Mesa Mesa { get; set; }
        public int MenuId { get; set; }
        public virtual Menu Menu { get; set; }
        public DateTime FechaHora { get; set; }
        public EstadoPedido Estado { get; set; }
        public double Precio { get; set; }
        public int Cantidad { get; set; }
    }
}
