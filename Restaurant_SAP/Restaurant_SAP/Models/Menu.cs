using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_SAP.Models
{
    public class Menu
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public double Precio { get; set; }
        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    }
}
