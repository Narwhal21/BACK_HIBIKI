using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Genero
    {
        public int GeneroId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = "";
        
        [MaxLength(500)]
        public string Descripcion { get; set; } = "";
        
        public string Color { get; set; } = "#ff5100"; // Color para el UI
        
        public string Icono { get; set; } = "🎵"; // Emoji o clase de icono

        public Genero() { }

        public Genero(string nombre, string descripcion = "", string color = "#ff5100", string icono = "🎵")
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre del género no puede estar vacío");
            }

            Nombre = nombre;
            Descripcion = descripcion;
            Color = color;
            Icono = icono;
        }
    }
}