using System.Collections.Generic;
using System.Web.Mvc;
using modular1.Models;
using modular1.Repositories;

namespace modular1.Controllers
{
    public class ClientesController : Controller
    {
        private ClienteRepository _clienteRepository = new ClienteRepository();

        public ActionResult Clientes()
        {
            return View();
        }

        // Obtener lista de clientes
        public JsonResult ListarClientes()
        {
            List<Cliente> clientes = _clienteRepository.Listar();
            return Json(new { data = clientes }, JsonRequestBehavior.AllowGet);
        }

        // Guardar un cliente (Registrar o Editar)
        [HttpPost]
        public JsonResult GuardarCliente(Cliente objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.IdCliente == 0)
            {
                // Convertir la clave a SHA256 antes de registrar
                string clave = Recursos.GenerarClave();

                // Preparar el asunto y el mensaje del correo
                string asunto = "Creación de Cuenta";
                string mensaje_correo = "<h3>Su cuenta fue creada correctamente</h3><br><p>Su contraseña para acceder es: !clave!</p>";
                mensaje_correo = mensaje_correo.Replace("!clave!", clave);

                bool respuesta = Recursos.EnviarCorreo(objeto.Correo, asunto, mensaje_correo);

                if (respuesta)
                {
                    // Establecer la clave encriptada antes de registrar
                    objeto.Clave = Recursos.ConvertirSha256(clave);
                    resultado = _clienteRepository.Registrar(objeto, out mensaje);
                }
                else
                {
                    mensaje = "No se puede enviar el correo";
                    return Json(new { resultado = false, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                // Editar cliente existente
                resultado = _clienteRepository.Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // Eliminar un cliente
        [HttpPost]
        public JsonResult EliminarCliente(int id)
        {
            string mensaje = string.Empty;
            bool resultado = _clienteRepository.Eliminar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}
