using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ControlCompras.Models
{
    public class Facturas
    {
        [Key]
        public int IdFactura { get; set; }

        public string NumeroFactura { get; set; }

        public decimal Total { get; set; }

        public DateTime FechaCompra { get; set; }

        public DateTime FechaRegistro { get; set; }

        public int UbicacionId { get; set; }

        [ForeignKey("UbicacionId")]
        public virtual Ubicaciones Ubicacion { get; set; }

        public virtual ICollection<DetalleFactura> DetalleFacturas { get; set; }
    }
}