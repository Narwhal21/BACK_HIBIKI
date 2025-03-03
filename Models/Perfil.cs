namespace Models
{
    using System;
    using System.Collections.Generic;

    public class Perfil
    {
        public int PerfilId { get; set; }
        public int UserId { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime UltimaActualizacion { get; set; }
        public int? CantanteId { get; set; }
        public int? CancionId { get; set; }
        public int? PlaylistId { get; set; }
        public List<Artista> ArtistasMasEscuchados { get; set; }
        public List<Cancion> CancionesMasEscuchadas { get; set; }

        public Perfil()
        {
            ArtistasMasEscuchados = new List<Artista>();
            CancionesMasEscuchadas = new List<Cancion>();
            FechaCreacion = DateTime.Now;
            UltimaActualizacion = DateTime.Now;
        }

        public Perfil(string nombre)
        {
            Nombre = nombre;
            ArtistasMasEscuchados = new List<Artista>();
            CancionesMasEscuchadas = new List<Cancion>();
            FechaCreacion = DateTime.Now;
            UltimaActualizacion = DateTime.Now;
        }
    }
}