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
            return await _albumRepository.GetAllAsync();  
        }

        public async Task<Album> GetByIdAsync(int id)
        {
            return await _albumRepository.GetByIdAsync(id);  
        }

        public async Task AddAsync(Album album)
        {
            if (album == null)
                throw new ArgumentNullException(nameof(album));

            await _albumRepository.AddAsync(album);  
        }

        public async Task UpdateAsync(Album album)
        {
            if (album == null)
                throw new ArgumentNullException(nameof(album));

            await _albumRepository.UpdateAsync(album);  
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _albumRepository.DeleteAsync(id);  
        }

        public async Task InitializeDataAsync()
        {
            await _albumRepository.InitializeDataAsync();  
        }

       
        public async Task<List<Album>> GetAlbumsByArtistIdAsync(int artistId)
        {
            return await _albumRepository.GetAlbumsByArtistIdAsync(artistId);  
        }
    }
}
