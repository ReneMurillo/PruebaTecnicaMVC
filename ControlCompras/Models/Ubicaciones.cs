using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ControlCompras.Models
{
    public class Ubicaciones
    {
        [Key]
        public int IdUbicacion { get; set; }

        public string Descripcion { get; set; }

        public virtual ICollection<Facturas> Facturas { get; set; }
    }
}