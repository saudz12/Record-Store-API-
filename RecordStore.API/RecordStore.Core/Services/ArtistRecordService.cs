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
    public class ArtistRecordService : IArtistRecordService
    {
        private readonly IArtistRecordRepository _artistRecordRepository;
        private readonly IMapper _mapper;

        public ArtistRecordService(IArtistRecordRepository artistRecordRepository, IMapper mapper)
        {
            _artistRecordRepository = artistRecordRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArtistRecordDto>> GetAllArtistRecordsAsync()
        {
            var artistRecords = await _artistRecordRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ArtistRecordDto>>(artistRecords);
        }

        public async Task<ArtistRecordDto> GetArtistRecordByIdAsync(int id)
        {
            var artistRecord = await _artistRecordRepository.GetByIdAsync(id);
            return _mapper.Map<ArtistRecordDto>(artistRecord);
        }

        public async Task<ArtistRecordDto> GetByArtistAndRecordAsync(int artistId, int recordId)
        {
            var artistRecord = await _artistRecordRepository.GetByArtistAndRecordAsync(artistId, recordId);
            return _mapper.Map<ArtistRecordDto>(artistRecord);
        }

        public async Task<IEnumerable<ArtistRecordDto>> GetByRecordIdAsync(int recordId)
        {
            var artistRecords = await _artistRecordRepository.GetByRecordIdAsync(recordId);
            return _mapper.Map<IEnumerable<ArtistRecordDto>>(artistRecords);
        }

        public async Task<IEnumerable<ArtistRecordDto>> GetByArtistIdAsync(int artistId)
        {
            var artistRecords = await _artistRecordRepository.GetByArtistIdAsync(artistId);
            return _mapper.Map<IEnumerable<ArtistRecordDto>>(artistRecords);
        }

        public async Task<ArtistRecordDto> CreateArtistRecordAsync(CreateArtistRecordDto createDto)
        {
            var existing = await _artistRecordRepository.GetByArtistAndRecordAsync(createDto.ArtistId, createDto.RecordId);
            if (existing != null)
            {
                throw new InvalidOperationException("Artist is already associated with this record");
            }

            var artistRecord = _mapper.Map<ArtistRecord>(createDto);
            var created = await _artistRecordRepository.CreateAsync(artistRecord);
            return _mapper.Map<ArtistRecordDto>(created);
        }

        public async Task<ArtistRecordDto> UpdateArtistRecordAsync(int id, UpdateArtistRecordDto updateDto)
        {
            var existing = await _artistRecordRepository.GetByIdAsync(id);
            if (existing == null) return null;

            _mapper.Map(updateDto, existing);
            var updated = await _artistRecordRepository.UpdateAsync(existing);
            return _mapper.Map<ArtistRecordDto>(updated);
        }

        public async Task<bool> DeleteArtistRecordAsync(int id)
        {
            return await _artistRecordRepository.DeleteAsync(id);
        }

        public async Task<bool> RemoveArtistFromRecordAsync(int artistId, int recordId)
        {
            return await _artistRecordRepository.DeleteByArtistAndRecordAsync(artistId, recordId);
        }
    }
}
