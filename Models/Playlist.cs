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
        
        public string Image { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string Descripcion { get; set; } = string.Empty; 
        
        public DateTime FechaCreacion { get; set; } = DateTime.Now; 
        
        // Lista de canciones en la playlist
        public List<Cancion> Canciones { get; set; } = new List<Cancion>();
        
        // Usuario creador - Permitir nulo inicialmente
        public Usuario? Creador { get; set; }

        public Playlist() 
        {
            // Constructor vacío para deserialización
        }

        public Playlist(int userId, int creadorId, string nombre, string descripcion = "", string image = "")
        {
            UserId = userId;
            CreadorId = creadorId;
            Nombre = nombre;
            Descripcion = descripcion;
            Image = image;
            FechaCreacion = DateTime.Now;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre de la playlist no puede estar vacío");
            }
        }
    }
}