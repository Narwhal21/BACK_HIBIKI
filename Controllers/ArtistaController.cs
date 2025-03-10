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

        [HttpGet]
        public async Task<ActionResult<List<Artista>>> GetAllArtistasAsync()
        {
            var artistas = await _artistaService.GetAllAsync();  
            return Ok(artistas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Artista>> GetArtistaByIdAsync(int id)
        {
            var artista = await _artistaService.GetByIdAsync(id);  
            if (artista == null)
                return NotFound();  
            return Ok(artista);
        }

      
        [HttpPost]
        public async Task<ActionResult> AddArtistaAsync([FromBody] Artista artista)
        {
            await _artistaService.AddAsync(artista); 
            return CreatedAtAction(nameof(GetArtistaByIdAsync), new { id = artista.CantanteId }, artista);  
        }

    
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateArtistaAsync(int id, [FromBody] Artista artista)
        {
            artista.CantanteId = id;
            await _artistaService.UpdateAsync(artista);  
            return NoContent();  
        }

   
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteArtistaAsync(int id)
        {
            var deleted = await _artistaService.DeleteAsync(id);  
            if (!deleted)
                return NotFound(); 
            return NoContent();  
        }

        [HttpPost("initialize")]
        public async Task<ActionResult> InitializeDataAsync()
        {
            await _artistaService.InitializeDataAsync();  
            return Ok();  
        }
    }
}