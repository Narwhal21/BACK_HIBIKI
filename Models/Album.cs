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
        if (artistId <= 0)
        {
            throw new ArgumentException("El ID del artista debe ser un valor positivo.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("El nombre del álbum no puede estar vacío");
        }

        if (string.IsNullOrWhiteSpace(image))
        {
            throw new ArgumentException("La imagen del álbum no puede estar vacía.");
        }

        ArtistId = artistId;
        Name = name;
        ReleaseDate = releaseDate;
        Image = image;
    }

}
