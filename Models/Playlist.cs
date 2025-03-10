using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Playlist
    {
        public int PlaylistId { get; set; } 
        
        public int UserId { get; set; } 
        
        public int CreadorId { get; set; } 
        

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty; 
        public string Image { get; set; } = "";
        [MaxLength(100)]
        public string Descripcion { get; set; } = string.Empty; 
        public DateTime FechaCreacion { get; set; } = DateTime.Now; 
        public List<Cancion> Canciones { get; set; } = new List<Cancion>();
        public Usuario Creador { get; set; }

        public Playlist() {}


        public Playlist(int playlistId, int userId, int creadorId, int cancionId, string nombre, string descripcion,string image, DateTime fechaCreacion, Usuario creador)
        {
            PlaylistId = playlistId;
            UserId = userId;
            CreadorId = creadorId;
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
