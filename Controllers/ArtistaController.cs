using Microsoft.AspNetCore.Mvc;
using MyMusicApp.Services;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistaController : ControllerBase
    {
        private readonly IArtistaService _artistaService;

        public ArtistaController(IArtistaService artistaService)
        {
            _artistaService = artistaService;
        }

        // Obtener todos los artistas
        [HttpGet]
        public async Task<ActionResult<List<Artista>>> GetAllArtistasAsync()
        {
            var artistas = await _artistaService.GetAllAsync();  // Llamada al método en el servicio
            return Ok(artistas);
        }

        // Obtener un artista por su ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Artista>> GetArtistaByIdAsync(int id)
        {
            var artista = await _artistaService.GetByIdAsync(id);  // Llamada al método en el servicio
            if (artista == null)
                return NotFound();  // Si no existe el artista, devuelve 404
            return Ok(artista);
        }

        // Agregar un nuevo artista
        [HttpPost]
        public async Task<ActionResult> AddArtistaAsync([FromBody] Artista artista)
        {
            await _artistaService.AddAsync(artista);  // Llamada al método en el servicio
            return CreatedAtAction(nameof(GetArtistaByIdAsync), new { id = artista.CantanteId }, artista);  // Devuelve el recurso creado con su ID
        }

        // Actualizar un artista existente
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateArtistaAsync(int id, [FromBody] Artista artista)
        {
            artista.CantanteId = id;
            await _artistaService.UpdateAsync(artista);  // Llamada al método en el servicio
            return NoContent();  // Devuelve 204 No Content cuando la actualización es exitosa
        }

        // Eliminar un artista
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteArtistaAsync(int id)
        {
            var deleted = await _artistaService.DeleteAsync(id);  // Llamada al método en el servicio
            if (!deleted)
                return NotFound();  // Si no se encuentra el artista, devuelve 404
            return NoContent();  // Devuelve 204 No Content cuando la eliminación es exitosa
        }

        // Inicializar los datos de artistas (si es necesario)
        [HttpPost("initialize")]
        public async Task<ActionResult> InitializeDataAsync()
        {
            await _artistaService.InitializeDataAsync();  // Llamada al método en el servicio
            return Ok();  // Retorna OK si se inicializan los datos correctamente
        }
    }
}