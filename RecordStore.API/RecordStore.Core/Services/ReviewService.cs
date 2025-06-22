using AutoMapper;
using RecordStore.Core.Dtos;
using RecordStore.Core.Services.Interfaces;
using RecordStore.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Core.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReviewDto>> GetAllReviewsAsync()
        {
            var reviews = await _reviewRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        public async Task<ReviewDto> GetReviewByIdAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            return _mapper.Map<ReviewDto>(review);
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsByRecordIdAsync(int recordId)
        {
            var reviews = await _reviewRepository.GetByRecordIdAsync(recordId);
            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsByUserIdAsync(int userId)
        {
            var reviews = await _reviewRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        public async Task<ReviewDto> GetReviewByUserAndRecordAsync(int userId, int recordId)
        {
            var review = await _reviewRepository.GetByUserAndRecordAsync(userId, recordId);
            return _mapper.Map<ReviewDto>(review);
        }

        public async Task<ReviewDto> CreateReviewAsync(CreateReviewDto createReviewDto)
        {
            // Check if user already reviewed this record
            var existingReview = await _reviewRepository.GetByUserAndRecordAsync(createReviewDto.UserId, createReviewDto.RecordId);
            if (existingReview != null)
            {
                throw new InvalidOperationException("User has already reviewed this record");
            }

            var review = _mapper.Map<Review>(createReviewDto);
            var createdReview = await _reviewRepository.CreateAsync(review);
            return _mapper.Map<ReviewDto>(createdReview);
        }

        public async Task<ReviewDto> UpdateReviewAsync(int id, UpdateReviewDto updateReviewDto)
        {
            var existingReview = await _reviewRepository.GetByIdAsync(id);
            if (existingReview == null) return null;

            _mapper.Map(updateReviewDto, existingReview);
            var updatedReview = await _reviewRepository.UpdateAsync(existingReview);
            return _mapper.Map<ReviewDto>(updatedReview);
        }

        public async Task<bool> DeleteReviewAsync(int id)
        {
            return await _reviewRepository.DeleteAsync(id);
        }

        public async Task<decimal> GetAverageRatingForRecordAsync(int recordId)
        {
            return await _reviewRepository.GetAverageRatingForRecordAsync(recordId);
        }
    }
}
