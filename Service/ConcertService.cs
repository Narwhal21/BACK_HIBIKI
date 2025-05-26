using Models;
using MyMusicApp.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace MyMusicApp.Services
{
    public class ConcertService : IConcertService
    {
        private readonly IConcertRepository _concertRepository;

        public ConcertService(IConcertRepository concertRepository)
        {
            _concertRepository = concertRepository;
        }

        public async Task<List<Concert>> GetAllAsync()
        {
            return await _concertRepository.GetAllAsync();
        }

        public async Task<Concert> GetByIdAsync(int id)
        {
            return await _concertRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Concert concert)
        {
            if (concert == null)
                throw new ArgumentNullException(nameof(concert));

            await _concertRepository.AddAsync(concert);
        }

        public async Task UpdateAsync(Concert concert)
        {
            if (concert == null)
                throw new ArgumentNullException(nameof(concert));

            await _concertRepository.UpdateAsync(concert);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _concertRepository.DeleteAsync(id);
        }

        public async Task InitializeDataAsync()
        {
            await _concertRepository.InitializeDataAsync();
        }
    }
}
