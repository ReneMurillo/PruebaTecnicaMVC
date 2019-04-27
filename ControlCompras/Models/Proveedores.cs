using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ControlCompras.Models
{
    public class Proveedores
    {
        [Key]
        public int IdProveedor { get; set; }

        public string RazonSocial { get; set; }

        public string Telefono { get; set; }

        public string Direccion { get; set; }

        public string Email { get; set; }

        public string Dui { get; set; }

        public string Nit { get; set; }

        public string Nrc { get; set; }

        public DateTime FechaRegistro { get; set; }

        public virtual ICollection<Productos> Productos { get; set; }
    }
}