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

    }
}
