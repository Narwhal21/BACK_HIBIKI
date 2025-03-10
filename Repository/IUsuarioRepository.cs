using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Repositories
{
    public interface IUsuarioRepository
    {

        Task<List<Usuario>> GetAllAsync();

        Task<Usuario> GetByIdAsync(int id);
        


        Task AddAsync(Usuario usuario);


        Task UpdateAsync(Usuario usuario);


        Task<bool> DeleteAsync(int id);
        Task<Usuario> GetByCredentialsAsync(string email, string password);

    }
}