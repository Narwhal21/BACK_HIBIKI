using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Repositories
{
    // Interfaz del repositorio para manejar usuarios
    public interface IUsuarioRepository
    {
        // Obtener todos los usuarios
        Task<List<Usuario>> GetAllAsync();

        // Obtener un usuario por su ID
        Task<Usuario> GetByIdAsync(int id);

        // Agregar un nuevo usuario
        Task AddAsync(Usuario usuario);

        // Actualizar un usuario existente
        Task UpdateAsync(Usuario usuario);

        // Eliminar un usuario
        Task<bool> DeleteAsync(int id);
    }
}