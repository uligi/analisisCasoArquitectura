using modular1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using modular1.Repositories;
using System.IO;

namespace modular1.Controllers
{
    public class ProductosController : Controller
    {

        //***************************************Categoria*************************************//

        private CategoriaRepository _categoriaRepository = new CategoriaRepository();
        // GET: Productos
        public ActionResult Categoria()
        {
            return View();
        }

        public JsonResult ListarCategorias()
        {
            List<Categoria> categorias = _categoriaRepository.Listar();
            return Json(new { data = categorias }, JsonRequestBehavior.AllowGet);
        }

        // Guardar una categoría (Registrar o Editar)
        [HttpPost]
        public JsonResult GuardarCategoria(Categoria objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.IdCategoria == 0)
            {
                // Registrar nueva categoría
                resultado = _categoriaRepository.Registrar(objeto, out mensaje);
            }
            else
            {
                // Editar categoría existente
                resultado = _categoriaRepository.Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // Eliminar una categoría
        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            string mensaje = string.Empty;
            bool resultado = _categoriaRepository.Eliminar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }



      //*******************************************Marca****************************************//

        private MarcaRepository _marcaRepository = new MarcaRepository();

        public ActionResult Marca()
        {
            return View();
        }

        public JsonResult ListarMarcas()
        {
            List<Marca> marcas = _marcaRepository.Listar();
            return Json(new { data = marcas }, JsonRequestBehavior.AllowGet);
        }

        // Guardar una marca (Registrar o Editar)
        [HttpPost]
        public JsonResult GuardarMarca(Marca objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.IdMarca == 0)
            {
                // Registrar nueva marca
                resultado = _marcaRepository.Registrar(objeto, out mensaje);
            }
            else
            {
                // Editar marca existente
                resultado = _marcaRepository.Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // Eliminar una marca
        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {
            string mensaje = string.Empty;
            bool resultado = _marcaRepository.Eliminar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }



        //****************************************Producto****************************************//

        private ProductoRepository _productoRepository = new ProductoRepository();
        public ActionResult Producto()
        {
            var marcas = _marcaRepository.Listar();
            var categorias = _categoriaRepository.Listar();
            ViewBag.Marcas = new SelectList(marcas, "IdMarca", "Descripcion");
            ViewBag.Categorias = new SelectList(categorias, "IdCategoria", "Descripcion");
            return View();
        }


        // Obtener lista de productos
        public JsonResult ListarProductos()
        {
            List<Producto> productos = _productoRepository.Listar();
            return Json(new { data = productos }, JsonRequestBehavior.AllowGet);
        }

        // Guardar un producto (Registrar o Editar)
        [HttpPost]

       
        public JsonResult GuardarProducto(Producto objeto, HttpPostedFileBase fileImagen)
        {
            string mensaje = string.Empty;
            object resultado;

            // Primer paso: Guardar o actualizar la información básica del producto
            if (objeto.IdProducto == 0)
            {
                resultado = _productoRepository.Registrar(objeto, out mensaje);
                objeto.IdProducto = (int)resultado; // Asumiendo que el resultado es el ID generado
            }
            else
            {
                resultado = _productoRepository.Editar(objeto, out mensaje);
            }

            // Segundo paso: Procesar y guardar la imagen si existe y el producto fue guardado correctamente
            // Segundo paso: Procesar y guardar la imagen si existe y el producto fue guardado correctamente
            if (fileImagen != null && fileImagen.ContentLength > 0 && resultado != null)
            {
                try
                {
                    var fileName = Path.GetFileName(fileImagen.FileName);
                    var folderPath = Server.MapPath("~/Images/UploadedImages/");
                    var path = Path.Combine(folderPath, fileName);

                    // Asegúrate de que la carpeta existe
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Guardar el archivo
                    fileImagen.SaveAs(path);

                    // Actualizar el objeto Producto con la ruta de la imagen
                    objeto.RutaImagen = "/Images/UploadedImages/" + fileName;
                    objeto.NombreImagen = fileName;

                    // Llamar a GuardarDatosImagen con el objeto actualizado
                    var updateResult = _productoRepository.GuardarDatosImagen(objeto, out mensaje);
                    if (!updateResult)
                    {
                        mensaje = "Producto guardado, pero la imagen no se pudo actualizar en la base de datos.";
                    }
                }
                catch (Exception ex)
                {
                    mensaje = "Error al guardar la imagen: " + ex.Message;
                    return Json(new { resultado = false, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
                }
            }


            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


         



        // Eliminar un producto
        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            string mensaje = string.Empty;
            bool resultado = _productoRepository.Eliminar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        //****************************** Vista de productos*******************************//


        public ActionResult VistaProductos()
        {
            List<Producto> productos = _productoRepository.Listar();
            return View(productos);
        }

    }
}