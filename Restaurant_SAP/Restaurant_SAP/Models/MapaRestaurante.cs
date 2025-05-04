using System.ComponentModel.DataAnnotations;

namespace Restaurant_SAP.Models
{
    public class MapaRestaurante
    {
        [Key]
        public int Id { get; set; }
        public int AnchoCuadricula { get; set; }
        public int AltoCuadricula { get; set; }
        // Podríamos agregar más propiedades como Nombre del Mapa, etc.
    }
}