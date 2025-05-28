using Microsoft.AspNetCore.Mvc;
using MyMusicApp.Services;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CancionController : ControllerBase
    {
        private readonly ICancionService _cancionService;

        public CancionController(ICancionService cancionService)
        {
            _cancionService = cancionService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cancion>>> GetAllCancionesAsync()
        {
            var canciones = await _cancionService.GetAllAsync();

            if (canciones == null || canciones.Count == 0)
            {
                return NotFound("No se encontraron canciones.");
            }

            return Ok(canciones);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cancion>> GetCancionByIdAsync(int id)
        {
            var cancion = await _cancionService.GetByIdAsync(id);

            if (cancion == null)
            {
                return NotFound($"No se encontró la canción con el ID {id}.");
            }

            return Ok(cancion);
        }

        [HttpGet("ByAlbum/{albumId}")]
        public async Task<ActionResult<List<Cancion>>> GetCancionesByAlbumIdAsync(int albumId)
        {
            var canciones = await _cancionService.GetCancionesByAlbumIdAsync(albumId);

            if (canciones == null || canciones.Count == 0)
            {
                return NotFound($"No se encontraron canciones para el álbum con ID {albumId}.");
            }

            return Ok(canciones);
        }

        // NUEVO: Endpoint para obtener canciones por cantante
        [HttpGet("ByCantante/{cantanteId}")]
        public async Task<ActionResult<List<Cancion>>> GetCancionesByCantanteIdAsync(int cantanteId)
        {
            var canciones = await _cancionService.GetCancionesByCantanteIdAsync(cantanteId);

            if (canciones == null || canciones.Count == 0)
            {
                return NotFound($"No se encontraron canciones para el cantante con ID {cantanteId}.");
            }

            return Ok(canciones);
        }

        // ENDPOINTS DE VIDEO ACTUALIZADOS Y NUEVOS

        // ACTUALIZADO: Endpoint para obtener canciones con video MP4 (para reproductor)
        [HttpGet("WithVideo")]
        public async Task<ActionResult<List<Cancion>>> GetCancionesWithVideoAsync()
        {
            var canciones = await _cancionService.GetCancionesWithVideoAsync();

            if (canciones == null || canciones.Count == 0)
            {
                return NotFound("No se encontraron canciones con video MP4.");
            }

            return Ok(canciones);
        }

        // NUEVO: Endpoint para obtener canciones con videoclip de YouTube
        [HttpGet("WithYouTube")]
        public async Task<ActionResult<List<Cancion>>> GetCancionesWithYouTubeAsync()
        {
            var canciones = await _cancionService.GetCancionesWithYouTubeAsync();

            if (canciones == null || canciones.Count == 0)
            {
                return NotFound("No se encontraron canciones con videoclip de YouTube.");
            }

            return Ok(canciones);
        }

        // NUEVO: Endpoint para obtener canciones con cualquier tipo de video
        [HttpGet("WithAnyVideo")]
        public async Task<ActionResult<List<Cancion>>> GetCancionesWithAnyVideoAsync()
        {
            var canciones = await _cancionService.GetCancionesWithAnyVideoAsync();

            if (canciones == null || canciones.Count == 0)
            {
                return NotFound("No se encontraron canciones con video.");
            }

            return Ok(canciones);
        }

        // NUEVO: Endpoint para obtener canciones con ambos tipos de video
        [HttpGet("WithBothVideos")]
        public async Task<ActionResult<List<Cancion>>> GetCancionesWithBothVideosAsync()
        {
            var canciones = await _cancionService.GetCancionesWithBothVideosAsync();

            if (canciones == null || canciones.Count == 0)
            {
                return NotFound("No se encontraron canciones con ambos tipos de video.");
            }

            return Ok(canciones);
        }

        // NUEVO: Endpoint para obtener estadísticas de videos
        [HttpGet("VideoStats")]
        public async Task<ActionResult<VideoStats>> GetVideoStatsAsync()
        {
            var stats = await _cancionService.GetVideoStatsAsync();
            return Ok(stats);
        }

        [HttpPost]
        public async Task<ActionResult> AddCancionAsync([FromBody] Cancion cancion)
        {
            if (cancion == null)
            {
                return BadRequest("La canción no puede ser nula.");
            }

            if (string.IsNullOrWhiteSpace(cancion.Nombre))
            {
                return BadRequest("El nombre de la canción es obligatorio.");
            }

            await _cancionService.AddAsync(cancion);
            return CreatedAtAction(nameof(GetCancionByIdAsync), new { id = cancion.CancionId }, cancion);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCancionAsync(int id, [FromBody] Cancion cancion)
        {
            if (cancion == null)
            {
                return BadRequest("La canción no puede ser nula.");
            }

            cancion.CancionId = id;

            var existingCancion = await _cancionService.GetByIdAsync(id);

            if (existingCancion == null)
            {
                return NotFound($"No se encontró la canción con el ID {id} para actualizar.");
            }

            await _cancionService.UpdateAsync(cancion);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCancionAsync(int id)
        {
            var deleted = await _cancionService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound($"No se encontró la canción con el ID {id} para eliminar.");
            }

            return NoContent();
        }
    }
}