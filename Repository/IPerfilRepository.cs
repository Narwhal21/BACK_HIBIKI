using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Repositories
{
    public interface IPerfilRepository
    {
        // Obtener todos los perfiles
        Task<List<Perfil>> GetAllAsync();

        // Obtener un perfil por su nombre
        Task<Perfil> GetByNombreAsync(string nombre);

        // Agregar un nuevo perfil
        Task AddAsync(Perfil perfil);

        // Actualizar un perfil existente
        Task UpdateAsync(Perfil perfil);

        // Eliminar un perfil por su nombre
        Task<bool> DeleteAsync(string nombre);

        // Inicializar datos de perfiles (opcional)
        Task InitializeDataAsync();
    }
}