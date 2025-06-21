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
    public class RecordRepository : IRecordRepository
    {
        private readonly RecordStoreDatabaseContext _context;

        public RecordRepository(RecordStoreDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Record>> GetAllAsync()
        {
            return await _context.Records
                .Include(r => r.ArtistRecords)
                    .ThenInclude(ar => ar.Artist)
                .Include(r => r.Inventories)
                .ToListAsync();
        }

        public async Task<Record> GetByIdAsync(int id)
        {
            return await _context.Records
                .Include(r => r.ArtistRecords)
                    .ThenInclude(ar => ar.Artist)
                .Include(r => r.Inventories)
                .FirstOrDefaultAsync(r => r.RecordId == id);
        }

        public async Task<Record> CreateAsync(Record record)
        {
            _context.Records.Add(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<Record> UpdateAsync(Record record)
        {
            _context.Entry(record).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var record = await _context.Records.FindAsync(id);
            if (record == null) return false;

            _context.Records.Remove(record);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Record>> GetByGenreAsync(string genre)
        {
            return await _context.Records
                .Where(r => r.Genre.Contains(genre))
                .Include(r => r.ArtistRecords)
                    .ThenInclude(ar => ar.Artist)
                .Include(r => r.Inventories)
                .ToListAsync();
        }

        public async Task<IEnumerable<Record>> SearchAsync(string searchTerm)
        {
            return await _context.Records
                .Where(r => r.Name.Contains(searchTerm) || r.Genre.Contains(searchTerm))
                .Include(r => r.ArtistRecords)
                    .ThenInclude(ar => ar.Artist)
                .Include(r => r.Inventories)
                .ToListAsync();
        }
    }
}
