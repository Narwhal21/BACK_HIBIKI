using Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyMusicApp.Repositories;
using System;

namespace MyMusicApp.Services
{
    public class ArtistaService : IArtistaService
    {
        private readonly IArtistaRepository _artistaRepository;

        public ArtistaService(IArtistaRepository artistaRepository)
        {
            _artistaRepository = artistaRepository;
        }

        public async Task<List<Artista>> GetAllAsync()
        {
            return await _artistaRepository.GetAllAsync();
        }

        public async Task<Artista> GetByIdAsync(int id)
        {
            return await _artistaRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Artista artista)
        {
            if (artista == null)
                throw new ArgumentNullException(nameof(artista));

            await _artistaRepository.AddAsync(artista);
        }

        public async Task UpdateAsync(Artista artista) 
        {
            if (artista == null)
                throw new ArgumentNullException(nameof(artista));

            await _artistaRepository.UpdateAsync(artista);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _artistaRepository.DeleteAsync(id);
        }

        public async Task InitializeDataAsync()
        {
            await _artistaRepository.InitializeDataAsync();
        }
    }
}
