using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace modular1.Models
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }

        [Required]
        [StringLength(500)]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        // Claves foráneas como propiedades enteras
        [ForeignKey("oMarca")]
        public int IdMarca { get; set; }

        [ForeignKey("oCategoria")]
        public int IdCategoria { get; set; }

        // Relaciones de objeto para navegación detallada en ORM
        public Marca oMarca { get; set; }
        public Categoria oCategoria { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; }

        public int Stock { get; set; }

        [StringLength(100)]
        public string RutaImagen { get; set; }

        [StringLength(100)]
        public string NombreImagen { get; set; }

        public bool Activo { get; set; }

        // Constructor para inicializar el objeto con valores predeterminados
        public Producto()
        {
            Activo = true;  // Asumiendo que por defecto los productos están activos
        }
    }
}
