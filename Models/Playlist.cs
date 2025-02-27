using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Playlist
    {
        public int PlaylistId { get; set; } // ID de la playlist
        
        public int UserId { get; set; } // ID del usuario propietario de la playlist
        
        public int CreadorId { get; set; } // ID del creador de la playlist
        
        public int CancionId { get; set; } // ID de la canción en la playlist (opcionalmente una lista)

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty; // Nombre de la playlist

        public string Image { get; set; } = "";
        
        [MaxLength(100)]
        public string Descripcion { get; set; } = string.Empty; // Descripción de la playlist
        
        public DateTime FechaCreacion { get; set; } = DateTime.Now; // Fecha de creación con valor predeterminado

        // Relaciones
        public List<Cancion> Canciones { get; set; } = new List<Cancion>();
        public Usuario Creador { get; set; }

        // Constructor sin parámetros
        public Playlist() {}


        // Constructor con parámetros
        public Playlist(int playlistId, int userId, int creadorId, int cancionId, string nombre, string descripcion,string image, DateTime fechaCreacion, Usuario creador)
        {
            PlaylistId = playlistId;
            UserId = userId;
            CreadorId = creadorId;
            CancionId = cancionId;
            Nombre = nombre;
            Descripcion = descripcion;
            Image = image;
            FechaCreacion = fechaCreacion;

            Creador = creador ?? throw new ArgumentException("El creador de la playlist no puede ser nulo");

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre de la playlist no puede estar vacío");
            }

            if (fechaCreacion == default)
            {
                throw new ArgumentException("La fecha de creación no puede ser nula");
            }
        }
    }
}
