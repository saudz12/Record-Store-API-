using RecordStore.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Database.Repositories.Interfaces
{
    public interface IOrderRecordRepository
    {
        Task<IEnumerable<OrderRecord>> GetAllAsync();
        Task<OrderRecord> GetByIdAsync(int id);
        Task<OrderRecord> GetByOrderAndRecordAsync(int orderId, int recordId);
        Task<IEnumerable<OrderRecord>> GetByOrderIdAsync(int orderId);
        Task<IEnumerable<OrderRecord>> GetByRecordIdAsync(int recordId);
        Task<OrderRecord> CreateAsync(OrderRecord orderRecord);
        Task<OrderRecord> UpdateAsync(OrderRecord orderRecord);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByOrderAndRecordAsync(int orderId, int recordId);
    }
}
