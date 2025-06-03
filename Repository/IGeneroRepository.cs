using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Repositories
{
    public interface IGeneroRepository
    {
        Task<List<Genero>> GetAllAsync();
        Task<Genero> GetByIdAsync(int id);
        Task AddAsync(Genero genero);
        Task UpdateAsync(Genero genero);
        Task<bool> DeleteAsync(int id);
        Task InitializeDataAsync();
    }
}