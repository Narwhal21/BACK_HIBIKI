using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Repositories
{

    public interface IArtistaRepository
    {

        Task<List<Artista>> GetAllAsync();


        Task<Artista> GetByIdAsync(int id);


        Task AddAsync(Artista artista);


        Task UpdateAsync(Artista artista);


        Task<bool> DeleteAsync(int id);

        Task InitializeDataAsync();
    }
}
