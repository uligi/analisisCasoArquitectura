using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Services.Description;
using modular1.Models;
using modular1.Repositories;

namespace modular1.Controllers
{
    public class UsuariosController : Controller
    {
        private UsuarioRepository _usuarioRepository = new UsuarioRepository();

        public ActionResult Usuarios()
        {
            return View();
        }

        // Obtener lista de usuarios
        public JsonResult ListarUsuarios()
        {
            List<Usuario> usuarios = _usuarioRepository.Listar();
            return Json(new { data = usuarios }, JsonRequestBehavior.AllowGet);
        }

        // Guardar un usuario (Registrar o Editar)
        [HttpPost]
        public JsonResult GuardarUsuario(Usuario objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.IdUsuario == 0)
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
                    resultado = _usuarioRepository.Registrar(objeto, out mensaje);
                }
                else
                {
                    // Si el correo no se envía, devolver una respuesta JSON con el mensaje de error
                    mensaje = "No se puede enviar el correo";
                    return Json(new { resultado = false, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                // Editar usuario existente
                resultado = _usuarioRepository.Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        // Eliminar un usuario
        [HttpPost]
        public JsonResult EliminarUsuario(int id)
        {
            string mensaje = string.Empty;
            bool resultado = _usuarioRepository.Eliminar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}
