using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecordStore.Core.Dtos;
using RecordStore.Core.Services.Interfaces;

namespace RecordStore.API.Controllers
{
    [ApiController]
    [Route("api/artist-records")]
    public class ArtistRecordsController : ControllerBase
    {
        private readonly IArtistRecordService _artistRecordService;

        public ArtistRecordsController(IArtistRecordService artistRecordService)
        {
            _artistRecordService = artistRecordService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtistRecordDto>>> GetArtistRecords()
        {
            var artistRecords = await _artistRecordService.GetAllArtistRecordsAsync();
            return Ok(artistRecords);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArtistRecordDto>> GetArtistRecord(int id)
        {
            var artistRecord = await _artistRecordService.GetArtistRecordByIdAsync(id);
            if (artistRecord == null)
                return NotFound();

            return Ok(artistRecord);
        }

        [HttpGet("artist/{artistId}/record/{recordId}")]
        public async Task<ActionResult<ArtistRecordDto>> GetByArtistAndRecord(int artistId, int recordId)
        {
            var artistRecord = await _artistRecordService.GetByArtistAndRecordAsync(artistId, recordId);
            if (artistRecord == null)
                return NotFound();

            return Ok(artistRecord);
        }

        [HttpGet("record/{recordId}")]
        public async Task<ActionResult<IEnumerable<ArtistRecordDto>>> GetByRecord(int recordId)
        {
            var artistRecords = await _artistRecordService.GetByRecordIdAsync(recordId);
            return Ok(artistRecords);
        }

        [HttpGet("artist/{artistId}")]
        public async Task<ActionResult<IEnumerable<ArtistRecordDto>>> GetByArtist(int artistId)
        {
            var artistRecords = await _artistRecordService.GetByArtistIdAsync(artistId);
            return Ok(artistRecords);
        }

        [HttpPost]
        public async Task<ActionResult<ArtistRecordDto>> CreateArtistRecord(CreateArtistRecordDto createDto)
        {
            try
            {
                var artistRecord = await _artistRecordService.CreateArtistRecordAsync(createDto);
                return CreatedAtAction(nameof(GetArtistRecord), new { id = artistRecord.Id }, artistRecord);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ArtistRecordDto>> UpdateArtistRecord(int id, UpdateArtistRecordDto updateDto)
        {
            var artistRecord = await _artistRecordService.UpdateArtistRecordAsync(id, updateDto);
            if (artistRecord == null)
                return NotFound();

            return Ok(artistRecord);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteArtistRecord(int id)
        {
            var success = await _artistRecordService.DeleteArtistRecordAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("artist/{artistId}/record/{recordId}")]
        public async Task<ActionResult> RemoveArtistFromRecord(int artistId, int recordId)
        {
            var success = await _artistRecordService.RemoveArtistFromRecordAsync(artistId, recordId);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
