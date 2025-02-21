using MyMusicApp.Repositories;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;

        public AlbumService(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }

        public async Task<List<Album>> GetAllAsync()
        {
            return await _albumRepository.GetAllAsync();  // Llamada al repositorio para obtener todos los álbumes
        }

        public async Task<Album> GetByIdAsync(int id)
        {
            return await _albumRepository.GetByIdAsync(id);  // Llamada al repositorio para obtener un álbum por su ID
        }

        public async Task AddAsync(Album album)
        {
            if (album == null)
                throw new ArgumentNullException(nameof(album));

            await _albumRepository.AddAsync(album);  // Llamada al repositorio para agregar un nuevo álbum
        }

        public async Task UpdateAsync(Album album)
        {
            if (album == null)
                throw new ArgumentNullException(nameof(album));

            await _albumRepository.UpdateAsync(album);  // Llamada al repositorio para actualizar un álbum
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _albumRepository.DeleteAsync(id);  // Llamada al repositorio para eliminar un álbum
        }

        public async Task InitializeDataAsync()
        {
            await _albumRepository.InitializeDataAsync();  // Llamada al repositorio para inicializar los datos
        }

        // Nuevo método para obtener los álbumes por el ID del artista
        public async Task<List<Album>> GetAlbumsByArtistIdAsync(int artistId)
        {
            return await _albumRepository.GetAlbumsByArtistIdAsync(artistId);  // Llamada al repositorio para obtener los álbumes por artistId
        }
    }
}
