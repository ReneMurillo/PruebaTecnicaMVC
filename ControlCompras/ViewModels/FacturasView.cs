using ControlCompras.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCompras.ViewModels
{
    public class FacturasView
    {

        public OrdenProductoView OrdenProducto { get; set; }

        public List<OrdenProductoView> Productos { get; set; }
    }
}