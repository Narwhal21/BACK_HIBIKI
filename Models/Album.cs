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
        ArtistId = artistId;
        Name = name;
        ReleaseDate = releaseDate;
        Image = image;

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("El nombre del álbum no puede estar vacío");
        }
    }

    // Métodos
}
