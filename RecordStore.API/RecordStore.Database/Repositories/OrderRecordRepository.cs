using Microsoft.EntityFrameworkCore;
using RecordStore.Database.Context;
using RecordStore.Database.Entities;
using RecordStore.Database.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Database.Repositories
{
    public class OrderRecordRepository : IOrderRecordRepository
    {
        private readonly RecordStoreDatabaseContext _context;

        public OrderRecordRepository(RecordStoreDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderRecord>> GetAllAsync()
        {
            return await _context.OrderRecords
                .Include(or => or.Order)
                    .ThenInclude(o => o.User)
                .Include(or => or.Record)
                    .ThenInclude(r => r.Inventories)
                .ToListAsync();
        }

        public async Task<OrderRecord> GetByIdAsync(int id)
        {
            return await _context.OrderRecords
                .Include(or => or.Order)
                    .ThenInclude(o => o.User)
                .Include(or => or.Record)
                    .ThenInclude(r => r.Inventories)
                .FirstOrDefaultAsync(or => or.Id == id);
        }

        public async Task<OrderRecord> GetByOrderAndRecordAsync(int orderId, int recordId)
        {
            return await _context.OrderRecords
                .Include(or => or.Order)
                .Include(or => or.Record)
                    .ThenInclude(r => r.Inventories)
                .FirstOrDefaultAsync(or => or.OrderId == orderId && or.RecordId == recordId);
        }

        public async Task<IEnumerable<OrderRecord>> GetByOrderIdAsync(int orderId)
        {
            return await _context.OrderRecords
                .Include(or => or.Record)
                    .ThenInclude(r => r.Inventories)
                .Where(or => or.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderRecord>> GetByRecordIdAsync(int recordId)
        {
            return await _context.OrderRecords
                .Include(or => or.Order)
                    .ThenInclude(o => o.User)
                .Include(or => or.Record)
                .Where(or => or.RecordId == recordId)
                .ToListAsync();
        }

        public async Task<OrderRecord> CreateAsync(OrderRecord orderRecord)
        {
            _context.OrderRecords.Add(orderRecord);
            await _context.SaveChangesAsync();
            return orderRecord;
        }

        public async Task<OrderRecord> UpdateAsync(OrderRecord orderRecord)
        {
            _context.Entry(orderRecord).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return orderRecord;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var orderRecord = await _context.OrderRecords.FindAsync(id);
            if (orderRecord == null) return false;

            _context.OrderRecords.Remove(orderRecord);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteByOrderAndRecordAsync(int orderId, int recordId)
        {
            var orderRecord = await GetByOrderAndRecordAsync(orderId, recordId);
            if (orderRecord == null) return false;

            _context.OrderRecords.Remove(orderRecord);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
