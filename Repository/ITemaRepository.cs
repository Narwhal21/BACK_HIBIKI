using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Repositories
{
    public interface ITemaRepository
    {
        Task<List<Tema>> GetAllAsync();
        Task<Tema> GetByIdAsync(int id);
        Task AddAsync(Tema tema);
        Task UpdateAsync(Tema tema);
        Task<bool> DeleteAsync(int id);
        Task<List<Tema>> GetTemasByCantanteIdAsync(int cantanteId);
    }
}
