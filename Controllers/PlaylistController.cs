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
            var playlists = await _playlistService.GetAllAsync();
            return Ok(playlists);
        }

        
        [HttpGet("{id}")]
public async Task<ActionResult<Playlist>> GetPlaylistByIdAsync(int id)
{
    try
    {
        var playlist = await _playlistService.GetByIdAsync(id);
        if (playlist == null)
        {
            Console.WriteLine($"Playlist {id} no encontrada.");
            return NotFound();
        }
        return Ok(playlist);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error en GetPlaylistByIdAsync({id}): {ex}");
        return StatusCode(500, "Error interno del servidor.");
    }
}



        
        [HttpPost]
        public async Task<ActionResult> AddPlaylistAsync([FromBody] Playlist playlist)
        {
            if (playlist == null || playlist.CreadorId <= 0 || playlist.UserId <= 0)
                return BadRequest("Datos de la playlist inválidos.");

            try
            {
                await _playlistService.AddAsync(playlist);
                return CreatedAtAction(nameof(GetPlaylistByIdAsync), new { id = playlist.PlaylistId }, playlist);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
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
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

      
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePlaylistAsync(int id)
        {
            var deleted = await _playlistService.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
