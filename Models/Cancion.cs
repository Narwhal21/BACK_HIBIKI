namespace Models;

public abstract class Cancion {

    public int CancionId { get; set; }
    public int AlbumId { get; set; }
    public string Nombre { get; set; } = "";
    public int Duracion { get; set; }
    public string Ruta { get; set; } = "";

    public Cancion() {}

    public Cancion(int albumId, string nombre, int duracion, string ruta) {
        AlbumId = albumId;
        Nombre = nombre;
        Duracion = duracion;
        Ruta = ruta;

        if (string.IsNullOrWhiteSpace(nombre)) {
            throw new ArgumentException("El nombre de la canción no puede estar vacío");
        }
        if (duracion <= 0) {
            throw new ArgumentException("La duración debe ser un número positivo");
        }
    }

    public abstract void MostrarDetalles();
}