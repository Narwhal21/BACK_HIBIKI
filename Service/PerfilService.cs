using Models;
using MyMusicApp.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Services
{
    public class PerfilService : IPerfilService
    {
        private readonly IPerfilRepository _perfilRepository;

        public PerfilService(IPerfilRepository perfilRepository)
        {
            _perfilRepository = perfilRepository;
        }

        public async Task<List<Perfil>> GetAllAsync()
        {
            return await _perfilRepository.GetAllAsync();
        }

        public async Task<Perfil> GetByNombreAsync(string nombre)
        {
            return await _perfilRepository.GetByNombreAsync(nombre);
        }

        public async Task AddAsync(Perfil perfil)
        {
            if (perfil == null)
                throw new ArgumentNullException(nameof(perfil));

            await _perfilRepository.AddAsync(perfil);
        }

        public async Task UpdateAsync(Perfil perfil)
        {
            if (perfil == null)
                throw new ArgumentNullException(nameof(perfil));

            await _perfilRepository.UpdateAsync(perfil);
        }

        public async Task<bool> DeleteAsync(string nombre)
        {
            return await _perfilRepository.DeleteAsync(nombre);
        }

        public async Task InitializeDataAsync()
        {
            await _perfilRepository.InitializeDataAsync();
        }
    }
}