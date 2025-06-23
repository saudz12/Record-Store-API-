using Microsoft.EntityFrameworkCore;
using RecordStore.Core.Dtos;
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

        public async Task<(IEnumerable<Record> Records, int TotalCount)> GetRecordsAsync(RecordQueryDto query)
        {
            var recordsQuery = _context.Records
                .Include(r => r.ArtistRecords)
                    .ThenInclude(ar => ar.Artist)
                .Include(r => r.Inventories)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                recordsQuery = recordsQuery.Where(r => r.Name.Contains(query.Name));
            }

            if (!string.IsNullOrWhiteSpace(query.Genre))
            {
                recordsQuery = recordsQuery.Where(r => r.Genre.Contains(query.Genre));
            }

            if (!string.IsNullOrWhiteSpace(query.RecordLabel))
            {
                recordsQuery = recordsQuery.Where(r => r.RecordLabel.Contains(query.RecordLabel));
            }

            if (!string.IsNullOrWhiteSpace(query.ArtistName))
            {
                recordsQuery = recordsQuery.Where(r => r.ArtistRecords
                    .Any(ar => ar.Artist.Name.Contains(query.ArtistName)));
            }

            if (query.ReleaseDateFrom.HasValue)
            {
                recordsQuery = recordsQuery.Where(r => r.ReleaseDate >= query.ReleaseDateFrom.Value);
            }

            if (query.ReleaseDateTo.HasValue)
            {
                recordsQuery = recordsQuery.Where(r => r.ReleaseDate <= query.ReleaseDateTo.Value);
            }

            if (query.MinDuration.HasValue)
            {
                recordsQuery = recordsQuery.Where(r => r.Duration >= query.MinDuration.Value);
            }

            if (query.MaxDuration.HasValue)
            {
                recordsQuery = recordsQuery.Where(r => r.Duration <= query.MaxDuration.Value);
            }

            if (query.MinPrice.HasValue)
            {
                recordsQuery = recordsQuery.Where(r => r.Inventories.Any(i => i.Price >= query.MinPrice.Value));
            }

            if (query.MaxPrice.HasValue)
            {
                recordsQuery = recordsQuery.Where(r => r.Inventories.Any(i => i.Price <= query.MaxPrice.Value));
            }

            if (query.InStock.HasValue)
            {
                if (query.InStock.Value)
                {
                    recordsQuery = recordsQuery.Where(r => r.Inventories.Any(i => i.Quantity > 0));
                }
                else
                {
                    recordsQuery = recordsQuery.Where(r => r.Inventories.All(i => i.Quantity == 0));
                }
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                recordsQuery = recordsQuery.Where(r =>
                    r.Name.Contains(query.SearchTerm) ||
                    r.Genre.Contains(query.SearchTerm) ||
                    r.RecordLabel.Contains(query.SearchTerm) ||
                    r.ArtistRecords.Any(ar => ar.Artist.Name.Contains(query.SearchTerm)));
            }

            recordsQuery = query.SortBy.ToLower() switch
            {
                "name" => query.SortOrder.ToLower() == "desc"
                    ? recordsQuery.OrderByDescending(r => r.Name)
                    : recordsQuery.OrderBy(r => r.Name),
                "releasedate" => query.SortOrder.ToLower() == "desc"
                    ? recordsQuery.OrderByDescending(r => r.ReleaseDate)
                    : recordsQuery.OrderBy(r => r.ReleaseDate),
                "duration" => query.SortOrder.ToLower() == "desc"
                    ? recordsQuery.OrderByDescending(r => r.Duration)
                    : recordsQuery.OrderBy(r => r.Duration),
                "genre" => query.SortOrder.ToLower() == "desc"
                    ? recordsQuery.OrderByDescending(r => r.Genre)
                    : recordsQuery.OrderBy(r => r.Genre),
                "recordlabel" => query.SortOrder.ToLower() == "desc"
                    ? recordsQuery.OrderByDescending(r => r.RecordLabel)
                    : recordsQuery.OrderBy(r => r.RecordLabel),
                "price" => query.SortOrder.ToLower() == "desc"
                    ? recordsQuery.OrderByDescending(r => r.Inventories.FirstOrDefault().Price)
                    : recordsQuery.OrderBy(r => r.Inventories.FirstOrDefault().Price),
                _ => recordsQuery.OrderBy(r => r.Name) // Default sorting
            };

            // Get total count before pagination
            var totalCount = await recordsQuery.CountAsync();

            // Apply pagination
            var records = await recordsQuery
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return (records, totalCount);
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
