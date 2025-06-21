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
    public class ArtistRepository : IArtistRepository
    {
        private readonly RecordStoreDatabaseContext _context;

        public ArtistRepository(RecordStoreDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Artist>> GetAllAsync()
        {
            return await _context.Artists.ToListAsync();
        }

        public async Task<Artist> GetByIdAsync(int id)
        {
            return await _context.Artists
                .Include(a => a.ArtistRecords)
                    .ThenInclude(ar => ar.Record)
                .FirstOrDefaultAsync(a => a.ArtistId == id);
        }

        public async Task<Artist> CreateAsync(Artist artist)
        {
            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();
            return artist;
        }

        public async Task<Artist> UpdateAsync(Artist artist)
        {
            _context.Entry(artist).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return artist;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var artist = await _context.Artists.FindAsync(id);
            if (artist == null) return false;

            _context.Artists.Remove(artist);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
