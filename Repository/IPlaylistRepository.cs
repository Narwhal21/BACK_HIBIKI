using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Repositories
{

    public interface IPlaylistRepository
    {

        Task<List<Playlist>> GetAllAsync();


        Task<Playlist> GetByIdAsync(int id);

        Task AddAsync(Playlist playlist);


        Task UpdateAsync(Playlist playlist);


        Task<bool> DeleteAsync(int id);
    }
}
