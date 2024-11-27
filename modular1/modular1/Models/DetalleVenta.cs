using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace modular1.Models
{
    public class DetalleVenta
    {
        public int IdDetalleVenta { get; set; }
        public int IdVenta { get; set; }
        public Producto oProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }

        public string IdTransaccion { get; set; }

    }
}