using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Services
{
    public interface IPlaylistService
    {
        Task<List<Playlist>> GetAllAsync();
        Task<Playlist> GetByIdAsync(int id);
        Task AddAsync(Playlist playlist);
        Task UpdateAsync(Playlist playlist);
        Task<bool> DeleteAsync(int id);
    }
}
