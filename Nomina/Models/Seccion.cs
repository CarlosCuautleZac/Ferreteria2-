using System;
using System.Collections.Generic;

#nullable disable

namespace Nomina.Models
{
    public partial class Seccion
    {
        public Seccion()
        {
            Productos = new HashSet<Producto>();
        }

        public int Numero { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Producto> Productos { get; set; }
    }
}
