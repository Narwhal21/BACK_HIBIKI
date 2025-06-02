using Microsoft.AspNetCore.Mvc;
using MyMusicApp.Services;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Playlist>>> GetAllPlaylistsAsync()
        {
            try
            {
                var playlists = await _playlistService.GetAllAsync();
                return Ok(playlists);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Playlist>> GetPlaylistByIdAsync(int id)
        {
            try
            {
                var playlist = await _playlistService.GetByIdAsync(id);
                if (playlist == null)
                {
                    return NotFound($"Playlist con ID {id} no encontrada.");
                }
                return Ok(playlist);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddPlaylistAsync([FromBody] Playlist playlist)
        {
            if (playlist == null)
                return BadRequest("Los datos de la playlist no pueden ser nulos.");

            try
            {
                await _playlistService.AddAsync(playlist);
                
                // CORREGIDO: Usar Ok en lugar de CreatedAtAction para evitar problemas de ruta
                return Ok(new { 
                    message = "Playlist creada exitosamente", 
                    playlistId = playlist.PlaylistId,
                    playlist = playlist 
                });
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePlaylistAsync(int id, [FromBody] Playlist playlist)
        {
            if (playlist == null || id != playlist.PlaylistId)
                return BadRequest("Datos inválidos.");

            try
            {
                await _playlistService.UpdateAsync(playlist);
                return NoContent();
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePlaylistAsync(int id)
        {
            try
            {
                var deleted = await _playlistService.DeleteAsync(id);
                if (!deleted)
                    return NotFound($"Playlist con ID {id} no encontrada.");
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // CORREGIDO: Endpoints para manejar canciones en playlists
        [HttpPost("{playlistId}/canciones/{cancionId}")]
        public async Task<ActionResult> AddCancionToPlaylistAsync(int playlistId, int cancionId)
        {
            try
            {
                var result = await _playlistService.AddCancionToPlaylistAsync(playlistId, cancionId);
                if (!result)
                    return BadRequest("No se pudo agregar la canción a la playlist. Es posible que ya exista.");
                
                return Ok(new { message = "Canción agregada exitosamente a la playlist." });
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpDelete("{playlistId}/canciones/{cancionId}")]
        public async Task<ActionResult> RemoveCancionFromPlaylistAsync(int playlistId, int cancionId)
        {
            try
            {
                var result = await _playlistService.RemoveCancionFromPlaylistAsync(playlistId, cancionId);
                if (!result)
                    return NotFound("La canción no se encontró en la playlist.");
                
                return Ok(new { message = "Canción eliminada exitosamente de la playlist." });
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // NUEVO: Endpoint para inicializar datos de prueba
        [HttpPost("initialize")]
        public async Task<ActionResult> InitializeDataAsync()
        {
            try
            {
                // Crear algunas playlists de prueba si no existen
                var playlists = await _playlistService.GetAllAsync();
                if (playlists.Count == 0)
                {
                    var playlist1 = new Playlist
                    {
                        CreadorId = 1,
                        UserId = 1,
                        Nombre = "Mi Primera Playlist",
                        Descripcion = "Una playlist de ejemplo",
                        Image = "https://placehold.co/300x300/444/fff?text=Playlist1"
                    };

                    var playlist2 = new Playlist
                    {
                        CreadorId = 1,
                        UserId = 1,
                        Nombre = "Favoritos",
                        Descripcion = "Mis canciones favoritas",
                        Image = "https://placehold.co/300x300/ff5100/fff?text=Favoritos"
                    };

                    await _playlistService.AddAsync(playlist1);
                    await _playlistService.AddAsync(playlist2);

                    return Ok(new { message = "Datos de prueba inicializados exitosamente." });
                }

                return Ok(new { message = "Ya existen playlists en la base de datos." });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}