using Microsoft.AspNetCore.Mvc;
using MyMusicApp.Services;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        // Obtener todos los álbumes
        [HttpGet]
        public async Task<ActionResult<List<Album>>> GetAllAlbumsAsync()
        {
            var albums = await _albumService.GetAllAsync();
            return Ok(albums);
        }

        // Obtener un álbum por su ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Album>> GetAlbumByIdAsync(int id)
        {
            var album = await _albumService.GetByIdAsync(id);
            if (album == null)
                return NotFound();
            return Ok(album);
        }

        // Obtener álbumes por el ID del artista
        [HttpGet("ByArtist/{artistId}")]
        public async Task<ActionResult<List<Album>>> GetAlbumsByArtistIdAsync(int artistId)
        {
            var albums = await _albumService.GetAlbumsByArtistIdAsync(artistId);  // Llamada al servicio
            if (albums == null || albums.Count == 0)
            {
                return NotFound("No se encontraron álbumes para este artista");
            }
            return Ok(albums); // Devuelve la lista de álbumes del artista
        }

        // Agregar un nuevo álbum
        [HttpPost]
        public async Task<ActionResult> AddAlbumAsync([FromBody] Album album)
        {
            await _albumService.AddAsync(album);
            return CreatedAtAction(nameof(GetAlbumByIdAsync), new { id = album.AlbumId }, album);
        }

        // Actualizar un álbum existente
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAlbumAsync(int id, [FromBody] Album album)
        {
            album.AlbumId = id;
            await _albumService.UpdateAsync(album);
            return NoContent();
        }

        // Eliminar un álbum
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAlbumAsync(int id)
        {
            var deleted = await _albumService.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }

        // Inicializar datos de álbumes (opcional)
        [HttpPost("initialize")]
        public async Task<ActionResult> InitializeDataAsync()
        {
            await _albumService.InitializeDataAsync();
            return Ok();
        }
    }
}
