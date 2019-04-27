using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ControlCompras.Models
{
    public class Marcas
    {
        [Key]
        public int IdMarca { get; set; }

        public string Descripcion { get; set; }

        public virtual ICollection<Productos> Productos { get; set; }
    }
}