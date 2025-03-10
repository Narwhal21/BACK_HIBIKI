using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Services
{
    public interface IUsuarioService
    {

        Task<List<Usuario>> GetAllAsync();


        Task<Usuario> GetByIdAsync(int id);

        Task AddAsync(Usuario usuario);


        Task UpdateAsync(Usuario usuario);


        Task<bool> DeleteAsync(int id);


        Task<Usuario> LoginAsync(string email, string password);
    }
}
