using Microsoft.AspNetCore.Mvc;
using MyMusicApp.Services;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemaController : ControllerBase
    {
        private readonly ITemaService _temaService;

        public TemaController(ITemaService temaService)
        {
            _temaService = temaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Tema>>> GetAllTemasAsync()
        {
            var temas = await _temaService.GetAllAsync();

            if (temas == null || temas.Count == 0)
            {
                return NotFound("No se encontraron temas.");
            }

            return Ok(temas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tema>> GetTemaByIdAsync(int id)
        {
            var tema = await _temaService.GetByIdAsync(id);

            if (tema == null)
            {
                return NotFound($"No se encontró el tema con el ID {id}.");
            }

            return Ok(tema);
        }

        [HttpGet("ByCantante/{cantanteId}")]
        public async Task<ActionResult<List<Tema>>> GetTemasByCantanteIdAsync(int cantanteId)
        {
            var temas = await _temaService.GetTemasByCantanteIdAsync(cantanteId);

            if (temas == null || temas.Count == 0)
            {
                return NotFound($"No se encontraron temas para el cantante con ID {cantanteId}.");
            }

            return Ok(temas);
        }

        [HttpPost]
        public async Task<ActionResult> AddTemaAsync([FromBody] Tema tema)
        {
            if (tema == null)
            {
                return BadRequest("El tema no puede ser nulo.");
            }

            if (string.IsNullOrWhiteSpace(tema.Nombre))
            {
                return BadRequest("El nombre del tema es obligatorio.");
            }

            await _temaService.AddAsync(tema);
            return CreatedAtAction(nameof(GetTemaByIdAsync), new { id = tema.TemaId }, tema);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTemaAsync(int id, [FromBody] Tema tema)
        {
            if (tema == null)
            {
                return BadRequest("El tema no puede ser nulo.");
            }

            tema.TemaId = id;

            var existingTema = await _temaService.GetByIdAsync(id);

            if (existingTema == null)
            {
                return NotFound($"No se encontró el tema con el ID {id} para actualizar.");
            }

            await _temaService.UpdateAsync(tema);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTemaAsync(int id)
        {
            var deleted = await _temaService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound($"No se encontró el tema con el ID {id} para eliminar.");
            }

            return NoContent();
        }
    }
}
