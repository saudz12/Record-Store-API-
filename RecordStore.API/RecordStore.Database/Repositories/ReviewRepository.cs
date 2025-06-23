using Microsoft.EntityFrameworkCore;
using RecordStore.Core.Services.Interfaces;
using RecordStore.Database.Context;
using RecordStore.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Database.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly RecordStoreDatabaseContext _context;

        public ReviewRepository(RecordStoreDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Record)
                .ToListAsync();
        }

        public async Task<Review> GetByIdAsync(int id)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Record)
                .FirstOrDefaultAsync(r => r.ReviewId == id);
        }

        public async Task<IEnumerable<Review>> GetByRecordIdAsync(int recordId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Record)
                .Where(r => r.RecordId == recordId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetByUserIdAsync(int userId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Record)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<Review> GetByUserAndRecordAsync(int userId, int recordId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Record)
                .FirstOrDefaultAsync(r => r.UserId == userId && r.RecordId == recordId);
        }

        public async Task<Review> CreateAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<Review> UpdateAsync(Review review)
        {
            _context.Entry(review).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetAverageRatingForRecordAsync(int recordId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.RecordId == recordId)
                .ToListAsync();

            return reviews.Any() ? reviews.Average(r => r.Rating) : 0;
        }
    }
}
