using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Repositories
{
    public interface IConcertRepository
    {
        Task<List<Concert>> GetAllAsync();

        Task<Concert> GetByIdAsync(int id);

        Task AddAsync(Concert concert);

        Task UpdateAsync(Concert concert);

        Task<bool> DeleteAsync(int id);

        Task InitializeDataAsync();
    }
}
