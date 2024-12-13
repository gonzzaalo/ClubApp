using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubApp.Models
{
    public class Socio
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El nombre completo no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "El apellido no puede superar los 100 caracteres.")]
        public string Apellido { get; set; } = string.Empty;

        [Phone]
        public string Telefono { get; set; } = string.Empty;

        [StringLength(150)]
        public string Direccion { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        // Relación con Cuota (Uno a Uno)
        public int CuotaId { get; set; }
        public Cuota? Cuota { get; set; }

        public override string ToString()
        {
            return $"{Nombre} {Apellido}";
        }
    }
}
