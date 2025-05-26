public class Concert
{
    public int ConcertId { get; set; }
    public int ArtistId { get; set; }
    public string Venue { get; set; } = "";
    public DateTime Date { get; set; }
    public string Description { get; set; } = "";
    public string Image { get; set; } = "";

    public Concert() {}

    public Concert(int artistId, string venue, DateTime date, string description, string image)
    {
        if (artistId <= 0)
        {
            throw new ArgumentException("El ID del artista debe ser un valor positivo.");
        }

        if (string.IsNullOrWhiteSpace(venue))
        {
            throw new ArgumentException("El lugar del concierto no puede estar vacío.");
        }

        if (date == default)
        {
            throw new ArgumentException("Debe proporcionarse una fecha válida para el concierto.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("La descripción no puede estar vacía.");
        }

        if (string.IsNullOrWhiteSpace(image))
        {
            throw new ArgumentException("La imagen del concierto no puede estar vacía.");
        }

        ArtistId = artistId;
        Venue = venue;
        Date = date;
        Description = description;
        Image = image;
    }
}
