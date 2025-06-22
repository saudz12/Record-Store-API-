using RecordStore.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Core.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryDto>> GetAllInventoryAsync();
        Task<InventoryDto> GetInventoryByIdAsync(int id);
        Task<InventoryDto> GetInventoryByRecordIdAsync(int recordId);
        Task<InventoryDto> CreateInventoryAsync(CreateInventoryDto createInventoryDto);
        Task<InventoryDto> UpdateInventoryAsync(int id, UpdateInventoryDto updateInventoryDto);
        Task<bool> DeleteInventoryAsync(int id);
        Task<bool> UpdateStockAsync(int recordId, int quantity);
    }
}
