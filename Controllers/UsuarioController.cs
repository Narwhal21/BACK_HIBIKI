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

       
        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> GetAllUsuariosAsync()
        {
            var usuarios = await _usuarioService.GetAllAsync();
            return Ok(usuarios);
        }
        [HttpGet("login")]
        public async Task<ActionResult<Usuario>> Login([FromQuery] string email, [FromQuery] string password)
        {
            var usuario = await _usuarioService.LoginAsync(email, password);
            if (usuario == null)
                return Unauthorized(new { message = "Credenciales inválidas" });
            return Ok(usuario);
        }




     
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuarioByIdAsync(int id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
                return NotFound();
            return Ok(usuario);
        }

    
        [HttpPost]
        public async Task<ActionResult> AddUsuarioAsync([FromBody] Usuario usuario)
        {
            try
            {
                await _usuarioService.AddAsync(usuario);

                if (usuario.UserId <= 0)
                {
                    return StatusCode(500, "Error interno: No se obtuvo el UserId después de la inserción.");
                }

                Console.WriteLine($"Usuario registrado con ID: {usuario.UserId}");

                return Ok(usuario); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar usuario: {ex.Message}");
                return StatusCode(500, $"Error interno en el servidor: {ex.Message}");
            }
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUsuarioAsync(int id, [FromBody] Usuario usuario)
        {
            usuario.UserId = id;
            await _usuarioService.UpdateAsync(usuario);
            return NoContent();
        }


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