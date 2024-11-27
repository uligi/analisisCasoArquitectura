using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace modular1.Conexion
{
    public static class ConexionDB
    {
        public static string cn = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;
    }
}
