namespace Models
{
   public class Artista
{
    public int CantanteId { get; set; }
    public string Nombre { get; set; }
    public int OyentesMensuales { get; set; }
    public string Descripcion { get; set; }
    public string Image { get; set; } = "";

public Artista() { }

        public Artista(string nombre, int oyentesMensuales, string descripcion, string image)
        {
            Nombre = nombre;
            OyentesMensuales = oyentesMensuales;
            Descripcion = descripcion;
            Image = image;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre no puede estar vacío");
            }
            if (oyentesMensuales < 0)
            {
                throw new ArgumentException("El número de oyentes mensuales no puede ser negativo");
            }
        }

        public void MostrarDetalles()
        {
            Console.WriteLine($"Nombre: {Nombre}, Oyentes Mensuales: {OyentesMensuales}, Descripcion: {Descripcion}");
        }
    }
}
