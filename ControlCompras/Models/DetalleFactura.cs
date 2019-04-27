using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ControlCompras.Models
{
    public class DetalleFactura
    {
        [Key]
        public int IdDetalle { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioCompra { get; set; }

        public decimal Total { get; set; }

        public int ProductoId { get; set; }

        public int FacturaId { get; set; }

        [ForeignKey("FacturaId")]
        public virtual Facturas Factura { get; set; }

        [ForeignKey("ProductoId")]
        public virtual Productos Producto { get; set; }
    }
}