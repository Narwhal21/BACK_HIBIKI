using Microsoft.AspNetCore.Mvc;
using MyMusicApp.Services;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MyMusicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ICancionService _cancionService;
        private readonly IArtistaService _artistaService;
        private readonly IAlbumService _albumService;
        private readonly IGeneroService _generoService;

        public SearchController(
            ICancionService cancionService, 
            IArtistaService artistaService, 
            IAlbumService albumService,
            IGeneroService generoService)
        {
            _cancionService = cancionService;
            _artistaService = artistaService;
            _albumService = albumService;
            _generoService = generoService;
        }

        // Búsqueda general
        [HttpGet]
        public async Task<ActionResult<SearchResult>> SearchAsync([FromQuery] string query = "", [FromQuery] string type = "all")
        {
            try
            {
                var result = new SearchResult();

                if (string.IsNullOrWhiteSpace(query))
                {
                    // Si no hay query, devolver todo
                    if (type == "all" || type == "canciones")
                        result.Canciones = await _cancionService.GetAllAsync();
                    if (type == "all" || type == "artistas")
                        result.Artistas = await _artistaService.GetAllAsync();
                    if (type == "all" || type == "albums")
                        result.Albums = await _albumService.GetAllAsync();
                }
                else
                {
                    var queryLower = query.ToLower();

                    if (type == "all" || type == "canciones")
                    {
                        var canciones = await _cancionService.GetAllAsync();
                        result.Canciones = canciones.Where(c => 
                            c.Nombre.ToLower().Contains(queryLower) ||
                            c.Artista.ToLower().Contains(queryLower) ||
                            c.Album.ToLower().Contains(queryLower) ||
                            c.Genero.ToLower().Contains(queryLower)
                        ).ToList();
                    }

                    if (type == "all" || type == "artistas")
                    {
                        var artistas = await _artistaService.GetAllAsync();
                        result.Artistas = artistas.Where(a => 
                            a.Nombre.ToLower().Contains(queryLower) ||
                            (a.Descripcion?.ToLower().Contains(queryLower) ?? false)
                        ).ToList();
                    }

                    if (type == "all" || type == "albums")
                    {
                        var albums = await _albumService.GetAllAsync();
                        result.Albums = albums.Where(a => 
                            a.Name.ToLower().Contains(queryLower)
                        ).ToList();
                    }
                }

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // Filtrar canciones por género
        [HttpGet("canciones/genero/{generoId}")]
        public async Task<ActionResult<List<Cancion>>> GetCancionesByGeneroAsync(int generoId)
        {
            try
            {
                var canciones = await _cancionService.GetCancionesByGeneroAsync(generoId);
                
                if (canciones == null || canciones.Count == 0)
                {
                    return NotFound($"No se encontraron canciones para el género con ID {generoId}.");
                }

                return Ok(canciones);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // Obtener todos los géneros
        [HttpGet("generos")]
        public async Task<ActionResult<List<Genero>>> GetAllGenerosAsync()
        {
            try
            {
                var generos = await _generoService.GetAllAsync();
                return Ok(generos);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // Búsqueda avanzada con filtros
        [HttpPost("advanced")]
        public async Task<ActionResult<List<Cancion>>> AdvancedSearchAsync([FromBody] AdvancedSearchRequest request)
        {
            try
            {
                var canciones = await _cancionService.GetAllAsync();

                // Aplicar filtros
                var filtered = canciones.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Query))
                {
                    var queryLower = request.Query.ToLower();
                    filtered = filtered.Where(c => 
                        c.Nombre.ToLower().Contains(queryLower) ||
                        c.Artista.ToLower().Contains(queryLower) ||
                        c.Album.ToLower().Contains(queryLower)
                    );
                }

                if (request.GeneroId.HasValue)
                {
                    filtered = filtered.Where(c => c.GeneroId == request.GeneroId.Value);
                }

                if (!string.IsNullOrWhiteSpace(request.Artista))
                {
                    var artistaLower = request.Artista.ToLower();
                    filtered = filtered.Where(c => c.Artista.ToLower().Contains(artistaLower));
                }

                if (request.DuracionMin.HasValue)
                {
                    filtered = filtered.Where(c => c.Duracion >= request.DuracionMin.Value);
                }

                if (request.DuracionMax.HasValue)
                {
                    filtered = filtered.Where(c => c.Duracion <= request.DuracionMax.Value);
                }

                if (request.ConVideo.HasValue)
                {
                    if (request.ConVideo.Value)
                        filtered = filtered.Where(c => c.TieneAlgunVideo);
                    else
                        filtered = filtered.Where(c => !c.TieneAlgunVideo);
                }

                if (request.ConLetra.HasValue)
                {
                    if (request.ConLetra.Value)
                        filtered = filtered.Where(c => c.TieneLetra);
                    else
                        filtered = filtered.Where(c => !c.TieneLetra);
                }

                var result = filtered.ToList();

                // Ordenar resultados
                switch (request.OrdenarPor?.ToLower())
                {
                    case "nombre":
                        result = result.OrderBy(c => c.Nombre).ToList();
                        break;
                    case "artista":
                        result = result.OrderBy(c => c.Artista).ToList();
                        break;
                    case "duracion":
                        result = result.OrderBy(c => c.Duracion).ToList();
                        break;
                    default:
                        // Mantener orden original
                        break;
                }

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }

    // Clases auxiliares
    public class SearchResult
    {
        public List<Cancion> Canciones { get; set; } = new List<Cancion>();
        public List<Artista> Artistas { get; set; } = new List<Artista>();
        public List<Album> Albums { get; set; } = new List<Album>();
    }

    public class AdvancedSearchRequest
    {
        public string? Query { get; set; }
        public int? GeneroId { get; set; }
        public string? Artista { get; set; }
        public System.TimeSpan? DuracionMin { get; set; }
        public System.TimeSpan? DuracionMax { get; set; }
        public bool? ConVideo { get; set; }
        public bool? ConLetra { get; set; }
        public string? OrdenarPor { get; set; } // "nombre", "artista", "duracion"
    }
}