using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecordStore.Core.Dtos;
using RecordStore.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace RecordStore.API.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryDto>>> GetInventory()
        {
            var inventory = await _inventoryService.GetAllInventoryAsync();
            return Ok(inventory);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryDto>> GetInventoryItem(int id)
        {
            var inventory = await _inventoryService.GetInventoryByIdAsync(id);
            if (inventory == null)
                return NotFound();

            return Ok(inventory);
        }

        [HttpGet("record/{recordId}")]
        public async Task<ActionResult<InventoryDto>> GetInventoryByRecord(int recordId)
        {
            var inventory = await _inventoryService.GetInventoryByRecordIdAsync(recordId);
            if (inventory == null)
                return NotFound();

            return Ok(inventory);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<InventoryDto>> CreateInventory(CreateInventoryDto createInventoryDto)
        {
            var inventory = await _inventoryService.CreateInventoryAsync(createInventoryDto);
            return CreatedAtAction(nameof(GetInventoryItem), new { id = inventory.Id }, inventory);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<InventoryDto>> UpdateInventory(int id, UpdateInventoryDto updateInventoryDto)
        {
            var inventory = await _inventoryService.UpdateInventoryAsync(id, updateInventoryDto);
            if (inventory == null)
                return NotFound();

            return Ok(inventory);
        }

        [Authorize]
        [HttpPut("stock/{recordId}")]
        public async Task<ActionResult> UpdateStock(int recordId, [FromBody] int quantity)
        {
            var success = await _inventoryService.UpdateStockAsync(recordId, quantity);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInventory(int id)
        {
            var success = await _inventoryService.DeleteInventoryAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
