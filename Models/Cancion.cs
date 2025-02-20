namespace Models
{
    public class Cancion
    {
        public int CancionId { get; set; }
        public int AlbumId { get; set; }
        public int CantanteId { get; set; }
        public string Nombre { get; set; } = "";
        public int Duracion { get; set; }
        public string Ruta { get; set; } = "";

        public Cancion() {}

        public Cancion(int albumId, int cantanteid, string nombre, int duracion, string ruta) 
        {
            AlbumId = albumId;
            CantanteId = cantanteid;
            Nombre = nombre;
            Duracion = duracion;
            Ruta = ruta;

            if (string.IsNullOrWhiteSpace(nombre)) 
            {
                throw new ArgumentException("El nombre de la canción no puede estar vacío");
            }

            if (duracion <= 0) 
            {
                throw new ArgumentException("La duración debe ser un número positivo");
            }
        }

        public void MostrarDetalles() 
        {
            // Implementación del método MostrarDetalles
        }
    }
}
