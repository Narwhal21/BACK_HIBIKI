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

            if (string.IsNullOrWhiteSpace(playlist.Nombre))
                throw new ArgumentException("El nombre de la playlist no puede estar vacío.");

            if (playlist.CreadorId <= 0 || playlist.UserId <= 0)
                throw new ArgumentException("CreadorId y UserId deben ser válidos.");

            playlist.FechaCreacion = DateTime.Now; // Se asegura de que la fecha sea válida

            await _playlistRepository.AddAsync(playlist);
        }

        public async Task UpdateAsync(Playlist playlist)
        {
            if (playlist == null)
                throw new ArgumentNullException(nameof(playlist));

            if (playlist.PlaylistId <= 0)
                throw new ArgumentException("El ID de la playlist no es válido.");

            if (string.IsNullOrWhiteSpace(playlist.Nombre))
                throw new ArgumentException("El nombre de la playlist no puede estar vacío.");

            await _playlistRepository.UpdateAsync(playlist);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de la playlist no es válido.");

            return await _playlistRepository.DeleteAsync(id);
        }
    }
}
