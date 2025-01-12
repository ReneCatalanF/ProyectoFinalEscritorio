using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_SAP.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public int MesaId { get; set; }
        public virtual Mesa Mesa { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }
    }
}
