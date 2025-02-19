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

        // Obtener todas las canciones
        [HttpGet]
        public async Task<ActionResult<List<Cancion>>> GetAllCancionesAsync()
        {
            var canciones = await _cancionService.GetAllAsync();
            return Ok(canciones);
        }

        // Obtener una canción por su ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Cancion>> GetCancionByIdAsync(int id)
        {
            var cancion = await _cancionService.GetByIdAsync(id);
            if (cancion == null)
                return NotFound();
            return Ok(cancion);
        }

        // Agregar una nueva canción
        [HttpPost]
        public async Task<ActionResult> AddCancionAsync([FromBody] Cancion cancion)
        {
            await _cancionService.AddAsync(cancion);
            return CreatedAtAction(nameof(GetCancionByIdAsync), new { id = cancion.CancionId }, cancion);
        }

        // Actualizar una canción existente
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCancionAsync(int id, [FromBody] Cancion cancion)
        {
            cancion.CancionId = id;
            await _cancionService.UpdateAsync(cancion);
            return NoContent();
        }

        // Eliminar una canción
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCancionAsync(int id)
        {
            var deleted = await _cancionService.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}