﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace modular1.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public bool Reestablecer { get; set; }

    }
}