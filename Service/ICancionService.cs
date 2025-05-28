using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Services
{
    public interface ICancionService
    {
        Task<List<Cancion>> GetAllAsync();
        Task<Cancion> GetByIdAsync(int id);
        Task AddAsync(Cancion cancion);
        Task UpdateAsync(Cancion cancion);
        Task<bool> DeleteAsync(int id);
        Task<List<Cancion>> GetCancionesByAlbumIdAsync(int albumId);
        Task<List<Cancion>> GetCancionesByCantanteIdAsync(int cantanteId); // NUEVO: Para obtener canciones por cantante
        Task<List<Cancion>> GetCancionesWithVideoAsync(); // NUEVO: Para obtener solo canciones con video
    }
}