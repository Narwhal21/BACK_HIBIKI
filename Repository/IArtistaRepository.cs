using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Repositories
{
    // Interfaz del repositorio para manejar artistas
    public interface IArtistaRepository
    {
        // Obtener todos los artistas
        Task<List<Artista>> GetAllAsync();

        // Obtener un artista por su ID
        Task<Artista> GetByIdAsync(int id);

        // Agregar un nuevo artista
        Task AddAsync(Artista artista);

        // Actualizar un artista existente
        Task UpdateAsync(Artista artista);

        // Eliminar un artista
        Task<bool> DeleteAsync(int id);

        // Inicializar datos de artistas (opcional)
        Task InitializeDataAsync();
    }
}
