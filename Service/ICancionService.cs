using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Services
{
    public interface ICancionService
    {
        Task<List<Cancion>> GetAllAsync();
        Task<Cancion> GetByIdAsync(int id);
        Task AddAsync(Cancion cancion);
        Task UpdateAsync(Cancion cancion);
        Task<bool> DeleteAsync(int id);
        Task<List<Cancion>> GetCancionesByAlbumIdAsync(int albumId);
        Task<List<Cancion>> GetCancionesByCantanteIdAsync(int cantanteId); // NUEVO: Para obtener canciones por cantante
        
        // MÉTODOS DE VIDEO ACTUALIZADOS
        Task<List<Cancion>> GetCancionesWithVideoAsync(); // MP4 para reproductor sincronizado
        Task<List<Cancion>> GetCancionesWithYouTubeAsync(); // NUEVO: YouTube para abrir externamente
        Task<List<Cancion>> GetCancionesWithAnyVideoAsync(); // NUEVO: Cualquier tipo de video
        Task<List<Cancion>> GetCancionesWithBothVideosAsync(); // NUEVO: Ambos tipos de video
        
        // ESTADÍSTICAS DE VIDEO
        Task<VideoStats> GetVideoStatsAsync(); // NUEVO: Estadísticas completas de videos
    }
}