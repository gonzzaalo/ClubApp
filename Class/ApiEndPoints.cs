using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubApp.Class
{
    public static class ApiEndPoints
    {
        public static string Deporte { get; set; } = "deportes";
        public static string Deportista { get; set; } = "deportistas";
        public static string Profesor { get; set; } = "profesores"; 
        public static string Socio { get; set; } = "socios";
        public static string Cuota { get; set; } = "cuotas";

        public static string GetEndPoint(string name)
        {
            return name switch
            {
                nameof(Deporte) => Deporte,
                nameof(Deportista) => Deportista,
                nameof(Profesor) => Profesor, 
                nameof(Socio) => Socio,
                nameof(Cuota) => Cuota,
                _ => throw new ArgumentException($"EndPoint '{name}' no está definido")
            };
        }
    }
}
