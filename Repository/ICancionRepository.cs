using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Repositories
{
    // Interfaz del repositorio para manejar canciones
    public interface ICancionRepository
    {
        // Obtener todas las canciones
        Task<List<Cancion>> GetAllAsync();

        // Obtener una canción por su ID
        Task<Cancion> GetByIdAsync(int id);

        // Agregar una nueva canción
        Task AddAsync(Cancion cancion);

        // Actualizar una canción existente
        Task UpdateAsync(Cancion cancion);

        // Eliminar una canción
        Task<bool> DeleteAsync(int id);
        Task<List<Cancion>> GetCancionesByAlbumIdAsync(int albumId);

    }
}
