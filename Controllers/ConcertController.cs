using Microsoft.AspNetCore.Mvc;
using MyMusicApp.Services;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConcertController : ControllerBase
    {
        private readonly IConcertService _concertService;

        public ConcertController(IConcertService concertService)
        {
            _concertService = concertService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Concert>>> GetAllConcertsAsync()
        {
            var concerts = await _concertService.GetAllAsync();
            return Ok(concerts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Concert>> GetConcertByIdAsync(int id)
        {
            var concert = await _concertService.GetByIdAsync(id);
            if (concert == null)
                return NotFound();
            return Ok(concert);
        }

        [HttpPost]
        public async Task<ActionResult> AddConcertAsync([FromBody] Concert concert)
        {
            await _concertService.AddAsync(concert);
            return CreatedAtAction(nameof(GetConcertByIdAsync), new { id = concert.ConcertId }, concert);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateConcertAsync(int id, [FromBody] Concert concert)
        {
            concert.ConcertId = id;
            await _concertService.UpdateAsync(concert);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteConcertAsync(int id)
        {
            var deleted = await _concertService.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }

        [HttpPost("initialize")]
        public async Task<ActionResult> InitializeDataAsync()
        {
            await _concertService.InitializeDataAsync();
            return Ok();
        }
    }
}
