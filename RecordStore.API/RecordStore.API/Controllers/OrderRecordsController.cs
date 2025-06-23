using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecordStore.Core.Dtos;
using RecordStore.Core.Services.Interfaces;

namespace RecordStore.API.Controllers
{
    
    [Route("api/order-records")]
    [ApiController]
    public class OrderRecordsController : ControllerBase
    {
        private readonly IOrderRecordService _orderRecordService;

        public OrderRecordsController(IOrderRecordService orderRecordService)
        {
            _orderRecordService = orderRecordService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderRecordDto>>> GetOrderRecords()
        {
            var orderRecords = await _orderRecordService.GetAllOrderRecordsAsync();
            return Ok(orderRecords);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderRecordDto>> GetOrderRecord(int id)
        {
            var orderRecord = await _orderRecordService.GetOrderRecordByIdAsync(id);
            if (orderRecord == null)
                return NotFound();

            return Ok(orderRecord);
        }

        [HttpGet("order/{orderId}/record/{recordId}")]
        public async Task<ActionResult<OrderRecordDto>> GetByOrderAndRecord(int orderId, int recordId)
        {
            var orderRecord = await _orderRecordService.GetByOrderAndRecordAsync(orderId, recordId);
            if (orderRecord == null)
                return NotFound();

            return Ok(orderRecord);
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<IEnumerable<OrderRecordDto>>> GetByOrder(int orderId)
        {
            var orderRecords = await _orderRecordService.GetByOrderIdAsync(orderId);
            return Ok(orderRecords);
        }

        [HttpGet("record/{recordId}")]
        public async Task<ActionResult<IEnumerable<OrderRecordDto>>> GetByRecord(int recordId)
        {
            var orderRecords = await _orderRecordService.GetByRecordIdAsync(recordId);
            return Ok(orderRecords);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderRecordDto>> CreateOrderRecord(CreateOrderRecordDto createDto)
        {
            try
            {
                var orderRecord = await _orderRecordService.CreateOrderRecordAsync(createDto);
                return CreatedAtAction(nameof(GetOrderRecord), new { id = orderRecord.Id }, orderRecord);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<ActionResult<OrderRecordDto>> UpdateOrderRecord(int id, UpdateOrderRecordDto updateDto)
        {
            try
            {
                var orderRecord = await _orderRecordService.UpdateOrderRecordAsync(id, updateDto);
                if (orderRecord == null)
                    return NotFound();

                return Ok(orderRecord);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrderRecord(int id)
        {
            var success = await _orderRecordService.DeleteOrderRecordAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [Authorize]
        [HttpDelete("order/{orderId}/record/{recordId}")]
        public async Task<ActionResult> RemoveRecordFromOrder(int orderId, int recordId)
        {
            var success = await _orderRecordService.RemoveRecordFromOrderAsync(orderId, recordId);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
