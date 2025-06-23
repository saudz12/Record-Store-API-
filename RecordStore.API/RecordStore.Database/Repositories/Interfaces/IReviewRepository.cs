using RecordStore.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Core.Services.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllAsync();
        Task<Review> GetByIdAsync(int id);
        Task<IEnumerable<Review>> GetByRecordIdAsync(int recordId);
        Task<IEnumerable<Review>> GetByUserIdAsync(int userId);
        Task<Review> GetByUserAndRecordAsync(int userId, int recordId);
        Task<Review> CreateAsync(Review review);
        Task<Review> UpdateAsync(Review review);
        Task<bool> DeleteAsync(int id);
        Task<decimal> GetAverageRatingForRecordAsync(int recordId);
    }
}
