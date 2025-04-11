using Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyMusicApp.Repositories;
using System;

namespace MyMusicApp.Services
{
    public class TemaService : ITemaService
    {
        private readonly ITemaRepository _temaRepository;

        public TemaService(ITemaRepository temaRepository)
        {
            _temaRepository = temaRepository;
        }

        public async Task<List<Tema>> GetAllAsync()
        {
            return await _temaRepository.GetAllAsync();
        }

        public async Task<Tema> GetByIdAsync(int id)
        {
            return await _temaRepository.GetByIdAsync(id);
        }

        public async Task<List<Tema>> GetTemasByCantanteIdAsync(int cantanteId)
        {
            if (cantanteId <= 0)
                throw new ArgumentException("El ID del cantante debe ser mayor a 0.");

            var temas = await _temaRepository.GetTemasByCantanteIdAsync(cantanteId);

            return temas ?? new List<Tema>(); // Retorna lista vacía si no hay resultados
        }

        public async Task AddAsync(Tema tema)
        {
            if (tema == null)
                throw new ArgumentNullException(nameof(tema));

            await _temaRepository.AddAsync(tema);
        }

        public async Task UpdateAsync(Tema tema)
        {
            if (tema == null)
                throw new ArgumentNullException(nameof(tema));

            await _temaRepository.UpdateAsync(tema);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _temaRepository.DeleteAsync(id);
        }
    }
}
