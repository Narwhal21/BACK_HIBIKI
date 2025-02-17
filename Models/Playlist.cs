namespace Models
{
    public class Playlist
    {
        public int PlaylistId { get; set; }
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public DateTime FechaCreacion { get; set; }
        public List<Cancion> Canciones { get; set; } = new List<Cancion>();
        public Usuario Creador { get; set; }

        public Playlist () {}

        public Playlist(int playlistId, string nombre, string descripcion, DateTime fechaCreacion, Usuario creador)
        {
            PlaylistId = playlistId;
            Nombre = nombre;
            Descripcion = descripcion;
            FechaCreacion = fechaCreacion;
            Creador = creador;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre de la lista de reproducción no puede estar vacío");
            }
            if (fechaCreacion == default)
            {
                throw new ArgumentException("La fecha de creación no puede ser nula");
            }
            if (creador == null)
            {
                throw new ArgumentException("El creador de la lista de reproducción no puede ser nulo");
            }
        }

    }
}

