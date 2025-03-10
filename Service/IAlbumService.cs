using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Services
{
    public interface IAlbumService
    {
        Task<List<Album>> GetAllAsync();           
        Task<Album> GetByIdAsync(int id);          
        Task AddAsync(Album album);               
        Task UpdateAsync(Album album);            
        Task<bool> DeleteAsync(int id);            
        Task InitializeDataAsync();              

        Task<List<Album>> GetAlbumsByArtistIdAsync(int artistId); 
    }
}
