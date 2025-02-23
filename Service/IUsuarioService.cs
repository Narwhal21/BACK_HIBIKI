using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Services
{
    // Interfaz del servicio para manejar usuarios
    public interface IUsuarioService
    {
        // Método para obtener todos los usuarios
        Task<List<Usuario>> GetAllAsync();

        // Método para obtener un usuario por su ID
        Task<Usuario> GetByIdAsync(int id);

        // Método para agregar un nuevo usuario
        Task AddAsync(Usuario usuario);

        // Método para actualizar un usuario existente
        Task UpdateAsync(Usuario usuario);

        // Método para eliminar un usuario por su ID
        Task<bool> DeleteAsync(int id);

        // Actualiza la firma de login para que retorne el usuario
        Task<Usuario> LoginAsync(string email, string password);
    }
}
