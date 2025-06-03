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

        // Método para obtener canciones por cantante
        public async Task<List<Cancion>> GetCancionesByCantanteIdAsync(int cantanteId)
        {
            if (cantanteId <= 0)
                throw new ArgumentException("El ID del cantante debe ser mayor a 0.");

            var canciones = await _cancionRepository.GetCancionesByCantanteIdAsync(cantanteId);

            return canciones ?? new List<Cancion>(); // Retorna lista vacía si no hay canciones
        }

        // ✅ IMPLEMENTADO: Método para obtener canciones por género
        public async Task<List<Cancion>> GetCancionesByGeneroAsync(int generoId)
        {
            if (generoId <= 0)
                throw new ArgumentException("El ID del género debe ser mayor a 0.");

            var canciones = await _cancionRepository.GetCancionesByGeneroAsync(generoId);

            return canciones ?? new List<Cancion>(); // Retorna lista vacía si no hay canciones
        }

        // Método para obtener canciones con video MP4 (para reproductor)
        public async Task<List<Cancion>> GetCancionesWithVideoAsync()
        {
            var todasLasCanciones = await _cancionRepository.GetAllAsync();
            return todasLasCanciones.Where(c => !string.IsNullOrEmpty(c.VideoUrl)).ToList();
        }

        // Método para obtener canciones con videoclip de YouTube
        public async Task<List<Cancion>> GetCancionesWithYouTubeAsync()
        {
            var todasLasCanciones = await _cancionRepository.GetAllAsync();
            return todasLasCanciones.Where(c => !string.IsNullOrEmpty(c.Videoclip)).ToList();
        }

        // Método para obtener canciones con cualquier tipo de video
        public async Task<List<Cancion>> GetCancionesWithAnyVideoAsync()
        {
            var todasLasCanciones = await _cancionRepository.GetAllAsync();
            return todasLasCanciones.Where(c => c.TieneAlgunVideo).ToList();
        }

        // Método para obtener canciones con ambos tipos de video
        public async Task<List<Cancion>> GetCancionesWithBothVideosAsync()
        {
            var todasLasCanciones = await _cancionRepository.GetAllAsync();
            return todasLasCanciones.Where(c => c.TieneAmbosVideos).ToList();
        }

        // Estadísticas de videos
        public async Task<VideoStats> GetVideoStatsAsync()
        {
            var todasLasCanciones = await _cancionRepository.GetAllAsync();
            
            return new VideoStats
            {
                TotalCanciones = todasLasCanciones.Count,
                ConVideoMP4 = todasLasCanciones.Count(c => c.TieneVideoMP4),
                ConVideoClipYouTube = todasLasCanciones.Count(c => c.TieneVideoClipYouTube),
                ConAmbosVideos = todasLasCanciones.Count(c => c.TieneAmbosVideos),
                SinVideo = todasLasCanciones.Count(c => !c.TieneAlgunVideo)
            };
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

    // Clase para estadísticas de videos
    public class VideoStats
    {
        public int TotalCanciones { get; set; }
        public int ConVideoMP4 { get; set; }
        public int ConVideoClipYouTube { get; set; }
        public int ConAmbosVideos { get; set; }
        public int SinVideo { get; set; }

        public double PorcentajeConMP4 => TotalCanciones > 0 ? (double)ConVideoMP4 / TotalCanciones * 100 : 0;
        public double PorcentajeConYouTube => TotalCanciones > 0 ? (double)ConVideoClipYouTube / TotalCanciones * 100 : 0;
        public double PorcentajeConAmbos => TotalCanciones > 0 ? (double)ConAmbosVideos / TotalCanciones * 100 : 0;
    }
}