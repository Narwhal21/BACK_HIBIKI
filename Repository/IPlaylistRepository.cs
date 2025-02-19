using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Repositories
{
    // Interfaz del repositorio para manejar playlists
    public interface IPlaylistRepository
    {
        // Obtener todas las playlists
        Task<List<Playlist>> GetAllAsync();

        // Obtener una playlist por su ID
        Task<Playlist> GetByIdAsync(int id);

        // Agregar una nueva playlist
        Task AddAsync(Playlist playlist);

        // Actualizar una playlist existente
        Task UpdateAsync(Playlist playlist);

        // Eliminar una playlist
        Task<bool> DeleteAsync(int id);
    }
}
