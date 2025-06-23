using AutoMapper;
using RecordStore.Core.Dtos;
using RecordStore.Core.Services.Interfaces;
using RecordStore.Database.Entities;
using RecordStore.Database.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Core.Services
{
    public class RecordService : IRecordService
    {
        private readonly IRecordRepository _recordRepository;
        private readonly IMapper _mapper;

        public RecordService(IRecordRepository recordRepository, IMapper mapper)
        {
            _recordRepository = recordRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RecordDto>> GetAllRecordsAsync()
        {
            var records = await _recordRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<RecordDto>>(records);
        }

        public async Task<RecordDto> GetRecordByIdAsync(int id)
        {
            var record = await _recordRepository.GetByIdAsync(id);
            return _mapper.Map<RecordDto>(record);
        }

        public async Task<RecordDto> CreateRecordAsync(CreateRecordDto createRecordDto)
        {
            var record = _mapper.Map<Record>(createRecordDto);
            var createdRecord = await _recordRepository.CreateAsync(record);
            return _mapper.Map<RecordDto>(createdRecord);
        }

        public async Task<RecordDto> UpdateRecordAsync(int id, UpdateRecordDto updateRecordDto)
        {
            var existingRecord = await _recordRepository.GetByIdAsync(id);
            if (existingRecord == null) return null;

            _mapper.Map(updateRecordDto, existingRecord);
            var updatedRecord = await _recordRepository.UpdateAsync(existingRecord);
            return _mapper.Map<RecordDto>(updatedRecord);
        }

        public async Task<bool> DeleteRecordAsync(int id)
        {
            return await _recordRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<RecordDto>> GetRecordsByGenreAsync(string genre)
        {
            var records = await _recordRepository.GetByGenreAsync(genre);
            return _mapper.Map<IEnumerable<RecordDto>>(records);
        }

        public async Task<IEnumerable<RecordDto>> SearchRecordsAsync(string searchTerm)
        {
            var records = await _recordRepository.SearchAsync(searchTerm);
            return _mapper.Map<IEnumerable<RecordDto>>(records);
        }
    }
}
