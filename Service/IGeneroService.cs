using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Services
{
    public interface IGeneroService
    {
        Task<List<Genero>> GetAllAsync();
        Task<Genero> GetByIdAsync(int id);
        Task AddAsync(Genero genero);
        Task UpdateAsync(Genero genero);
        Task<bool> DeleteAsync(int id);
        Task InitializeDataAsync();
    }
}