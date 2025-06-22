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
    public class ArtistRecordRepository : IArtistRecordRepository
    {
        private readonly RecordStoreDatabaseContext _context;

        public ArtistRecordRepository(RecordStoreDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ArtistRecord>> GetAllAsync()
        {
            return await _context.ArtistRecords
                .Include(ar => ar.Artist)
                .Include(ar => ar.Record)
                .ToListAsync();
        }

        public async Task<ArtistRecord> GetByIdAsync(int id)
        {
            return await _context.ArtistRecords
                .Include(ar => ar.Artist)
                .Include(ar => ar.Record)
                .FirstOrDefaultAsync(ar => ar.Id == id);
        }

        public async Task<ArtistRecord> GetByArtistAndRecordAsync(int artistId, int recordId)
        {
            return await _context.ArtistRecords
                .Include(ar => ar.Artist)
                .Include(ar => ar.Record)
                .FirstOrDefaultAsync(ar => ar.ArtistId == artistId && ar.RecordId == recordId);
        }

        public async Task<IEnumerable<ArtistRecord>> GetByRecordIdAsync(int recordId)
        {
            return await _context.ArtistRecords
                .Include(ar => ar.Artist)
                .Include(ar => ar.Record)
                .Where(ar => ar.RecordId == recordId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ArtistRecord>> GetByArtistIdAsync(int artistId)
        {
            return await _context.ArtistRecords
                .Include(ar => ar.Artist)
                .Include(ar => ar.Record)
                .Where(ar => ar.ArtistId == artistId)
                .ToListAsync();
        }

        public async Task<ArtistRecord> CreateAsync(ArtistRecord artistRecord)
        {
            _context.ArtistRecords.Add(artistRecord);
            await _context.SaveChangesAsync();
            return artistRecord;
        }

        public async Task<ArtistRecord> UpdateAsync(ArtistRecord artistRecord)
        {
            _context.Entry(artistRecord).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return artistRecord;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var artistRecord = await _context.ArtistRecords.FindAsync(id);
            if (artistRecord == null) return false;

            _context.ArtistRecords.Remove(artistRecord);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteByArtistAndRecordAsync(int artistId, int recordId)
        {
            var artistRecord = await GetByArtistAndRecordAsync(artistId, recordId);
            if (artistRecord == null) return false;

            _context.ArtistRecords.Remove(artistRecord);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
