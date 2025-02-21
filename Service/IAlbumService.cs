using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Services
{
    public interface IAlbumService
    {
        Task<List<Album>> GetAllAsync();           // Método para obtener todos los álbumes
        Task<Album> GetByIdAsync(int id);          // Método para obtener un álbum por su ID
        Task AddAsync(Album album);                // Método para agregar un nuevo álbum
        Task UpdateAsync(Album album);             // Método para actualizar un álbum
        Task<bool> DeleteAsync(int id);            // Método para eliminar un álbum
        Task InitializeDataAsync();               // Método para inicializar datos (ejemplo de prueba)

        // Nuevo método para obtener álbumes por el ID del artista
        Task<List<Album>> GetAlbumsByArtistIdAsync(int artistId); // Método para obtener los álbumes por el artistId
    }
}
