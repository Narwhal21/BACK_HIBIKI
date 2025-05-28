using Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyMusicApp.Repositories;
using System;
using System.Linq;

namespace MyMusicApp.Services
{
    public class CancionService : ICancionService
    {
        private readonly ICancionRepository _cancionRepository;

        public CancionService(ICancionRepository cancionRepository)
        {
            _cancionRepository = cancionRepository;
        }

        public async Task<List<Cancion>> GetAllAsync()
        {
            return await _cancionRepository.GetAllAsync();
        }

        public async Task<Cancion> GetByIdAsync(int id)
        {
            return await _cancionRepository.GetByIdAsync(id);
        }

        public async Task<List<Cancion>> GetCancionesByAlbumIdAsync(int albumId)
        {
            if (albumId <= 0)
                throw new ArgumentException("El ID del álbum debe ser mayor a 0.");

            var canciones = await _cancionRepository.GetCancionesByAlbumIdAsync(albumId);

            return canciones ?? new List<Cancion>(); // Retorna lista vacía si no hay canciones
        }

        // NUEVO: Método para obtener canciones por cantante
        public async Task<List<Cancion>> GetCancionesByCantanteIdAsync(int cantanteId)
        {
            if (cantanteId <= 0)
                throw new ArgumentException("El ID del cantante debe ser mayor a 0.");

            var canciones = await _cancionRepository.GetCancionesByCantanteIdAsync(cantanteId);

            return canciones ?? new List<Cancion>(); // Retorna lista vacía si no hay canciones
        }

        // NUEVO: Método para obtener solo canciones con video
        public async Task<List<Cancion>> GetCancionesWithVideoAsync()
        {
            var todasLasCanciones = await _cancionRepository.GetAllAsync();
            return todasLasCanciones.Where(c => !string.IsNullOrEmpty(c.VideoUrl)).ToList();
        }

        public async Task AddAsync(Cancion cancion)
        {
            if (cancion == null)
                throw new ArgumentNullException(nameof(cancion));

            await _cancionRepository.AddAsync(cancion);
        }

        public async Task UpdateAsync(Cancion cancion)
        {
            if (cancion == null)
                throw new ArgumentNullException(nameof(cancion));

            await _cancionRepository.UpdateAsync(cancion);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _cancionRepository.DeleteAsync(id);
        }
    }
}