using Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyMusicApp.Repositories;
using System;

namespace MyMusicApp.Services
{
    public class GeneroService : IGeneroService
    {
        private readonly IGeneroRepository _generoRepository;

        public GeneroService(IGeneroRepository generoRepository)
        {
            _generoRepository = generoRepository;
        }

        public async Task<List<Genero>> GetAllAsync()
        {
            return await _generoRepository.GetAllAsync();
        }

        public async Task<Genero> GetByIdAsync(int id)
        {
            return await _generoRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Genero genero)
        {
            if (genero == null)
                throw new ArgumentNullException(nameof(genero));

            if (string.IsNullOrWhiteSpace(genero.Nombre))
                throw new ArgumentException("El nombre del género no puede estar vacío.");

            await _generoRepository.AddAsync(genero);
        }

        public async Task UpdateAsync(Genero genero)
        {
            if (genero == null)
                throw new ArgumentNullException(nameof(genero));

            if (string.IsNullOrWhiteSpace(genero.Nombre))
                throw new ArgumentException("El nombre del género no puede estar vacío.");

            await _generoRepository.UpdateAsync(genero);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _generoRepository.DeleteAsync(id);
        }

        public async Task InitializeDataAsync()
        {
            await _generoRepository.InitializeDataAsync();
        }
    }
}