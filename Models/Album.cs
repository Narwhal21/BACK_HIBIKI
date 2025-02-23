public class Album
{
    public int AlbumId { get; set; }
    public int ArtistId { get; set; }
    public string Name { get; set; } = "";
    public DateTime? ReleaseDate { get; set; }
    public string Image { get; set; } = "";

    public Album() {}

    public Album(int artistId, string name, DateTime? releaseDate, string image)
    {
        // Validación para ArtistId: asegurarse de que sea positivo.
        if (artistId <= 0)
        {
            throw new ArgumentException("El ID del artista debe ser un valor positivo.");
        }

        // Validación para Name: asegurarse de que no esté vacío o nulo.
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("El nombre del álbum no puede estar vacío");
        }

        // Validación para Image: asegurarse de que la imagen no esté vacía.
        if (string.IsNullOrWhiteSpace(image))
        {
            throw new ArgumentException("La imagen del álbum no puede estar vacía.");
        }

        ArtistId = artistId;
        Name = name;
        ReleaseDate = releaseDate;
        Image = image;
    }

    // Métodos adicionales si los necesitas...
}
