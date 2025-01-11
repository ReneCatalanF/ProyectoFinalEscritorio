using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_SAP.Models
{
    public enum EstadoMesa { Libre, Ocupada, Reservada, Pronto }
    public class Mesa
    {
        [Key]
        public int Id { get; set; }
        public int Numero { get; set; }
        public string Descripcion { get; set; }
        public EstadoMesa Estado { get; set; }
        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

        public void Reservar() { Estado = EstadoMesa.Reservada; }
        public void Liberar() { Estado = EstadoMesa.Libre; }
    }
}
