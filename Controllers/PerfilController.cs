using Microsoft.AspNetCore.Mvc;
using MyMusicApp.Services;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilController : ControllerBase
    {
        private readonly IPerfilService _perfilService;

        public PerfilController(IPerfilService perfilService)
        {
            _perfilService = perfilService;
        }

        // Obtener todos los perfiles
        [HttpGet]
        public async Task<ActionResult<List<Perfil>>> GetAllPerfilesAsync()
        {
            var perfiles = await _perfilService.GetAllAsync();
            return Ok(perfiles);
        }

        // Obtener un perfil por su nombre
        [HttpGet("{nombre}")]
        public async Task<ActionResult<Perfil>> GetPerfilByNombreAsync(string nombre)
        {
            var perfil = await _perfilService.GetByNombreAsync(nombre);
            if (perfil == null)
                return NotFound();
            return Ok(perfil);
        }

        // Agregar un nuevo perfil
        [HttpPost]
        public async Task<ActionResult> AddPerfilAsync([FromBody] Perfil perfil)
        {
            await _perfilService.AddAsync(perfil);
            return CreatedAtAction(nameof(GetPerfilByNombreAsync), new { nombre = perfil.Nombre }, perfil);
        }

        // Actualizar un perfil existente
        [HttpPut("{nombre}")]
        public async Task<ActionResult> UpdatePerfilAsync(string nombre, [FromBody] Perfil perfil)
        {
            if (nombre != perfil.Nombre)
            {
                return BadRequest("El nombre del perfil no coincide.");
            }

            var existingPerfil = await _perfilService.GetByNombreAsync(nombre);
            if (existingPerfil == null)
            {
                return NotFound($"No se encontró un perfil con el nombre {nombre}");
            }

            // Mantener el ID de la base de datos
            perfil.PerfilId = existingPerfil.PerfilId;
            
            await _perfilService.UpdateAsync(perfil);
            return NoContent();
        }

        // Eliminar un perfil
        [HttpDelete("{nombre}")]
        public async Task<ActionResult> DeletePerfilAsync(string nombre)
        {
            var deleted = await _perfilService.DeleteAsync(nombre);
            if (!deleted)
                return NotFound();
            return NoContent();
        }

        // Inicializar datos de perfiles (si es necesario)
        [HttpPost("initialize")]
        public async Task<ActionResult> InitializeDataAsync()
        {
            await _perfilService.InitializeDataAsync();
            return Ok();
        }
    }
}