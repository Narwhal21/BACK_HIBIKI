using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Repositories
{
    public interface IPerfilRepository
    {
    
        Task<List<Perfil>> GetAllAsync();

        Task<Perfil> GetByNombreAsync(string nombre);


        Task AddAsync(Perfil perfil);

    
        Task UpdateAsync(Perfil perfil);


        Task<bool> DeleteAsync(string nombre);


        Task InitializeDataAsync();
    }
}