using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubApp.ViewModels
{
    public class ProfesorDeporteViewModel
    {
        public int ProfesorId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string DeporteNombre { get; set; } // Nombre del deporte
    }
}
