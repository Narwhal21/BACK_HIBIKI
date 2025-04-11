namespace Models
{
    public class Tema
    {
        public int TemaId { get; set; }
        public int CantanteId { get; set; }
        public string Nombre { get; set; } = "";
        public TimeSpan Duracion { get; set; }
        public string Ruta { get; set; } = "";
        public string Image { get; set; } = "";

        public Tema() {}

        public Tema(int cantanteid, string nombre, TimeSpan duracion, string ruta, string image) 
        {
            CantanteId = cantanteid;
            Nombre = nombre;
            Duracion = duracion;
            Ruta = ruta;
            Image = image;

            if (string.IsNullOrWhiteSpace(nombre)) 
            {
                throw new ArgumentException("El nombre de la canción no puede estar vacío");
            }
        }

        public void MostrarDetalles() 
        {
        }
    }
}
