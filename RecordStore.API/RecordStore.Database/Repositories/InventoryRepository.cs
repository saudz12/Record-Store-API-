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
    public class InventoryRepository : IInventoryRepository
    {
        private readonly RecordStoreDatabaseContext _context;

        public InventoryRepository(RecordStoreDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Inventory>> GetAllAsync()
        {
            return await _context.Inventories
                .Include(i => i.Record)
                .ToListAsync();
        }

        public async Task<Inventory> GetByIdAsync(int id)
        {
            return await _context.Inventories
                .Include(i => i.Record)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Inventory> GetByRecordIdAsync(int recordId)
        {
            return await _context.Inventories
                .FirstOrDefaultAsync(i => i.RecordId == recordId);
        }

        public async Task<Inventory> CreateAsync(Inventory inventory)
        {
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();
            return inventory;
        }

        public async Task<Inventory> UpdateAsync(Inventory inventory)
        {
            _context.Entry(inventory).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return inventory;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null) return false;

            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStockAsync(int recordId, int quantity)
        {
            var inventory = await GetByRecordIdAsync(recordId);
            if (inventory == null) return false;

            inventory.Quantity = quantity;
            await UpdateAsync(inventory);
            return true;
        }
    }
}
