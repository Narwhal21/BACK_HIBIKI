using Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyMusicApp.Repositories;
using System;

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
