using RecordStore.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Core.Services.Interfaces
{
    public interface IOrderRecordService
    {
        Task<IEnumerable<OrderRecordDto>> GetAllOrderRecordsAsync();
        Task<OrderRecordDto> GetOrderRecordByIdAsync(int id);
        Task<OrderRecordDto> GetByOrderAndRecordAsync(int orderId, int recordId);
        Task<IEnumerable<OrderRecordDto>> GetByOrderIdAsync(int orderId);
        Task<IEnumerable<OrderRecordDto>> GetByRecordIdAsync(int recordId);
        Task<OrderRecordDto> CreateOrderRecordAsync(CreateOrderRecordDto createDto);
        Task<OrderRecordDto> UpdateOrderRecordAsync(int id, UpdateOrderRecordDto updateDto);
        Task<bool> DeleteOrderRecordAsync(int id);
        Task<bool> RemoveRecordFromOrderAsync(int orderId, int recordId);
    }
}
