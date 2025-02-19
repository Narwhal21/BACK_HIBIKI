using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Services
{
    // Interfaz del servicio para manejar canciones
    public interface ICancionService
    {
        // Método para obtener todas las canciones
        Task<List<Cancion>> GetAllAsync();

        // Método para obtener una canción por su ID
        Task<Cancion> GetByIdAsync(int id);

        // Método para agregar una nueva canción
        Task AddAsync(Cancion cancion);

        // Método para actualizar una canción existente
        Task UpdateAsync(Cancion cancion);

        // Método para eliminar una canción por su ID
        Task<bool> DeleteAsync(int id);
    }
}
