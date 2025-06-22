using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecordStore.Core.Dtos;
using RecordStore.Core.Services.Interfaces;

namespace RecordStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public ArtistsController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtistDto>>> GetArtists()
        {
            var artists = await _artistService.GetAllArtistsAsync();
            return Ok(artists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArtistDto>> GetArtist(int id)
        {
            var artist = await _artistService.GetArtistByIdAsync(id);
            if (artist == null)
                return NotFound();

            return Ok(artist);
        }

        [HttpPost]
        public async Task<ActionResult<ArtistDto>> CreateArtist(CreateArtistDto createArtistDto)
        {
            var artist = await _artistService.CreateArtistAsync(createArtistDto);
            return CreatedAtAction(nameof(GetArtist), new { id = artist.ArtistId }, artist);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ArtistDto>> UpdateArtist(int id, UpdateArtistDto updateArtistDto)
        {
            var artist = await _artistService.UpdateArtistAsync(id, updateArtistDto);
            if (artist == null)
                return NotFound();

            return Ok(artist);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteArtist(int id)
        {
            var success = await _artistService.DeleteArtistAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
