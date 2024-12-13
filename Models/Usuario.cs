using ClubApp.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubApp.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string User { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public TipoUsuarioEnum TipoUsuario { get; set; }
        public int? SocioId { get; set; }
        public Socio? Socio { get; set; }
        public bool Eliminado { get; set; }
        public override string ToString()
        {
            return Socio != null ? $"{Socio}" : string.Empty;
        }
    }
}
