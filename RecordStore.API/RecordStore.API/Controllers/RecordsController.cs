﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecordStore.Core.Dtos;
using RecordStore.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace RecordStore.API.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class RecordsController : ControllerBase
    {
        private readonly IRecordService _recordService;

        public RecordsController(IRecordService recordService)
        {
            _recordService = recordService;
        }

        [HttpGet("query")]
        public async Task<ActionResult<PagedResultDto<RecordDto>>> GetRecordsWithQuery([FromQuery] RecordQueryDto query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _recordService.GetRecordsAsync(query);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecordDto>>> GetRecords()
        {
            var records = await _recordService.GetAllRecordsAsync();
            return Ok(records);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecordDto>> GetRecord(int id)
        {
            var record = await _recordService.GetRecordByIdAsync(id);
            if (record == null)
                return NotFound();

            return Ok(record);
        }

        [HttpGet("genre/{genre}")]
        public async Task<ActionResult<IEnumerable<RecordDto>>> GetRecordsByGenre(string genre)
        {
            var records = await _recordService.GetRecordsByGenreAsync(genre);
            return Ok(records);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<RecordDto>>> SearchRecords([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return BadRequest("Search term is required");

            var records = await _recordService.SearchRecordsAsync(term);
            return Ok(records);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<RecordDto>> CreateRecord(CreateRecordDto createRecordDto)
        {
            var record = await _recordService.CreateRecordAsync(createRecordDto);
            return CreatedAtAction(nameof(GetRecord), new { id = record.RecordId }, record);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<RecordDto>> UpdateRecord(int id, UpdateRecordDto updateRecordDto)
        {
            var record = await _recordService.UpdateRecordAsync(id, updateRecordDto);
            if (record == null)
                return NotFound();

            return Ok(record);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRecord(int id)
        {
            var success = await _recordService.DeleteRecordAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
