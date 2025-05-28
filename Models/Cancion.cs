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
        public string? Videoclip { get; set; } = null; // NUEVO: YouTube para abrir externamente

        public string Letra { get; set; } = "";

<<<<<<< HEAD
        public Cancion(int albumId, int cantanteid, string nombre, TimeSpan duracion, string ruta, string image, string? videoUrl = null, string? videoclip = null)
=======
        public string Videoclip { get; set; } = "";
        

        public Cancion() { }

        public Cancion(int albumId, int cantanteid, string nombre, TimeSpan duracion, string ruta, string image, string letra, string videoclip, string? videoUrl = null)
>>>>>>> bf29f10322204a85337e8d69fd3c779271b9de86
        {
            AlbumId = albumId;
            CantanteId = cantanteid;
            Nombre = nombre;
            Duracion = duracion;
            Ruta = ruta;
            Image = image;
<<<<<<< HEAD
            VideoUrl = videoUrl; // MP4 para reproductor sincronizado
            Videoclip = videoclip; // NUEVO: YouTube para ver externamente
=======
            VideoUrl = videoUrl; // NUEVO: Asignar videoUrl
            Letra = letra;
            Videoclip = videoclip;
>>>>>>> bf29f10322204a85337e8d69fd3c779271b9de86

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre de la canción no puede estar vacío");
            }
        }

        public void MostrarDetalles()
        {
            Console.WriteLine($"Nombre: {Nombre}, Duración: {Duracion}, Tiene MP4: {!string.IsNullOrEmpty(VideoUrl)}, Tiene YouTube: {!string.IsNullOrEmpty(Videoclip)}");
        }

        // NUEVO: Métodos de utilidad para videos
        public bool TieneVideoMP4 => !string.IsNullOrEmpty(VideoUrl);
        public bool TieneVideoClipYouTube => !string.IsNullOrEmpty(Videoclip);
        public bool TieneAlgunVideo => TieneVideoMP4 || TieneVideoClipYouTube;
        public bool TieneAmbosVideos => TieneVideoMP4 && TieneVideoClipYouTube;
    }
}