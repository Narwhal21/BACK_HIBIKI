using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Repositories
{
    public interface ICancionRepository
    {
        Task<List<Cancion>> GetAllAsync();
        Task<Cancion> GetByIdAsync(int id);
        Task AddAsync(Cancion cancion);
        Task UpdateAsync(Cancion cancion);
        Task<bool> DeleteAsync(int id);
        Task<List<Cancion>> GetCancionesByAlbumIdAsync(int albumId);
        Task<List<Cancion>> GetCancionesByCantanteIdAsync(int cantanteId); 
    }
}