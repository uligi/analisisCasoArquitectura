using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace modular1.Models
{
    public class Carrito
    {
        public int IdCarrito { get; set; }
        public Cliente oCliente { get; set; }
        public Producto oProducto { get; set; }
        public int Cantidad { get; set; }

    }
}