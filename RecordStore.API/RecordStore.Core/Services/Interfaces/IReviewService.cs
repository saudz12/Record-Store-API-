using RecordStore.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Core.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetAllReviewsAsync();
        Task<ReviewDto> GetReviewByIdAsync(int id);
        Task<IEnumerable<ReviewDto>> GetReviewsByRecordIdAsync(int recordId);
        Task<IEnumerable<ReviewDto>> GetReviewsByUserIdAsync(int userId);
        Task<ReviewDto> GetReviewByUserAndRecordAsync(int userId, int recordId);
        Task<ReviewDto> CreateReviewAsync(CreateReviewDto createReviewDto);
        Task<ReviewDto> UpdateReviewAsync(int id, UpdateReviewDto updateReviewDto);
        Task<bool> DeleteReviewAsync(int id);
        Task<decimal> GetAverageRatingForRecordAsync(int recordId);
    }
}
