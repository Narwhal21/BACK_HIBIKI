using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Services
{
    public interface ITemaService
    {
        Task<List<Tema>> GetAllAsync();

        Task<Tema> GetByIdAsync(int id);

        Task AddAsync(Tema tema);

        Task UpdateAsync(Tema tema);

        Task<bool> DeleteAsync(int id);

        Task<List<Tema>> GetTemasByCantanteIdAsync(int cantanteId);
    }
}
