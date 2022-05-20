using System;
using System.Collections.Generic;

#nullable disable

namespace Nomina.Models
{
    public partial class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
        public int Seccion { get; set; }

        public virtual Seccion SeccionNavigation { get; set; }
    }
}
