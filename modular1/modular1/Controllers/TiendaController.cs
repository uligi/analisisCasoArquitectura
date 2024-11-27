using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace modular1.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Carrito()
        {
            return View();
        }
        public ActionResult DetalleProducto()
        {
            return View();
        }

        public ActionResult PagoEfectuado()
        {
            return View();
        }
    }
}