using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyMusicApp.Repositories;

namespace MyMusicApp.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;

        public PlaylistService(IPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }

        public async Task<List<Playlist>> GetAllAsync()
        {
            return await _playlistRepository.GetAllAsync();
        }

        public async Task<Playlist> GetByIdAsync(int id)
        {
            return await _playlistRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Playlist playlist)
        {
            if (playlist == null)
                throw new ArgumentNullException(nameof(playlist));

            // Validación adicional si fuera necesario, por ejemplo, verificar si el nombre ya existe.
            await _playlistRepository.AddAsync(playlist);
        }

        public async Task UpdateAsync(Playlist playlist)
        {
            if (playlist == null)
                throw new ArgumentNullException(nameof(playlist));

            // Validación adicional si fuera necesario.
            await _playlistRepository.UpdateAsync(playlist);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _playlistRepository.DeleteAsync(id);
        }
    }
}
