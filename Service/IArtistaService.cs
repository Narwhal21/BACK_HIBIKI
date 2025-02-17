using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Services
{
    // Interfaz del servicio para manejar artistas
    public interface IArtistaService
    {
        // Método para obtener todos los artistas
        Task<List<Artista>> GetAllAsync();

        // Método para obtener un artista por su ID
        Task<Artista> GetByIdAsync(int id);

        // Método para agregar un nuevo artista
        Task AddAsync(Artista artista);

        // Método para actualizar un artista existente
        Task UpdateAsync(Artista artista);

        // Método para eliminar un artista por su ID
        Task<bool> DeleteAsync(int id);

        // Método para inicializar datos de artistas (si se requiere)
        Task InitializeDataAsync();
    }
}
