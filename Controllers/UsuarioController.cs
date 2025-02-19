using Microsoft.AspNetCore.Mvc;
using MyMusicApp.Services;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // Obtener todos los usuarios
        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> GetAllUsuariosAsync()
        {
            var usuarios = await _usuarioService.GetAllAsync();
            return Ok(usuarios);
        }

        // Obtener un usuario por su ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuarioByIdAsync(int id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
                return NotFound();
            return Ok(usuario);
        }

        // Agregar un nuevo usuario
        [HttpPost]
        public async Task<ActionResult> AddUsuarioAsync([FromBody] Usuario usuario)
        {
            await _usuarioService.AddAsync(usuario);
            return CreatedAtAction(nameof(GetUsuarioByIdAsync), new { id = usuario.UserId }, usuario);
        }

        // Actualizar un usuario existente
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUsuarioAsync(int id, [FromBody] Usuario usuario)
        {
            usuario.UserId = id;
            await _usuarioService.UpdateAsync(usuario);
            return NoContent();
        }

        // Eliminar un usuario
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUsuarioAsync(int id)
        {
            var deleted = await _usuarioService.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}