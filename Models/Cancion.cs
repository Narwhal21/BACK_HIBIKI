using System;

namespace Models
{
    public class Cancion
    {
        public int CancionId { get; set; }
        public int AlbumId { get; set; }
        public int CantanteId { get; set; }
        public string Nombre { get; set; } = "";
        public TimeSpan Duracion { get; set; }
        public string Ruta { get; set; } = "";
        public string Image { get; set; } = "";
        public string? VideoUrl { get; set; } = null; // MP4 para sincronización con reproductor
        public string Letra { get; set; } = ""; // Letra de la canción
        public string Videoclip { get; set; } = ""; // YouTube para abrir externamente

        public Cancion() { }

        public Cancion(int albumId, int cantanteid, string nombre, TimeSpan duracion, string ruta, string image, string letra, string videoclip, string? videoUrl = null)
        {
            AlbumId = albumId;
            CantanteId = cantanteid;
            Nombre = nombre;
            Duracion = duracion;
            Ruta = ruta;
            Image = image;
            VideoUrl = videoUrl; // MP4 para reproductor sincronizado
            Letra = letra; // Letra de la canción
            Videoclip = videoclip; // YouTube para ver externamente

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre de la canción no puede estar vacío");
            }
        }

        public void MostrarDetalles()
        {
            Console.WriteLine($"Nombre: {Nombre}, Duración: {Duracion}, Tiene MP4: {!string.IsNullOrEmpty(VideoUrl)}, Tiene YouTube: {!string.IsNullOrEmpty(Videoclip)}, Tiene Letra: {!string.IsNullOrEmpty(Letra)}");
        }

        // Métodos de utilidad para videos
        public bool TieneVideoMP4 => !string.IsNullOrEmpty(VideoUrl);
        public bool TieneVideoClipYouTube => !string.IsNullOrEmpty(Videoclip);
        public bool TieneAlgunVideo => TieneVideoMP4 || TieneVideoClipYouTube;
        public bool TieneAmbosVideos => TieneVideoMP4 && TieneVideoClipYouTube;
        
        // NUEVO: Método de utilidad para letra
        public bool TieneLetra => !string.IsNullOrEmpty(Letra);
    }
}