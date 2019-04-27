using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ControlCompras.Models
{
    public class Productos
    {
        [Key]
        public int IdProducto { get; set; }

        public string BarCode { get; set; }

        public string Nombre { get; set; }

        public int Stock { get; set; }

        public decimal Precio { get; set; }

        public string Descripcion { get; set; }

        public DateTime FechaRegistro { get; set; }

        public int ProveedorId { get; set; }

        public int CategoriaId { get; set; }

        public int MarcaId { get; set; }


        [ForeignKey("MarcaId")]
        public virtual Marcas Marca { get; set; }

        [ForeignKey("CategoriaId")]
        public virtual Categorias Categoria { get; set; }

        [ForeignKey("ProveedorId")]
        public virtual Proveedores Proveedor { get; set; }

        public virtual ICollection<DetalleFactura> DetalleFacturas { get; set; }
    }
}