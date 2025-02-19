using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Services
{
    // Interfaz del servicio para manejar playlists
    public interface IPlaylistService
    {
        // Método para obtener todas las playlists
        Task<List<Playlist>> GetAllAsync();

        // Método para obtener una playlist por su ID
        Task<Playlist> GetByIdAsync(int id);

        // Método para agregar una nueva playlist
        Task AddAsync(Playlist playlist);

        // Método para actualizar una playlist existente
        Task UpdateAsync(Playlist playlist);

        // Método para eliminar una playlist por su ID
        Task<bool> DeleteAsync(int id);
    }
}
