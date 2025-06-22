using RecordStore.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Core.Services.Interfaces
{
    public interface IArtistRecordService
    {
        Task<IEnumerable<ArtistRecordDto>> GetAllArtistRecordsAsync();
        Task<ArtistRecordDto> GetArtistRecordByIdAsync(int id);
        Task<ArtistRecordDto> GetByArtistAndRecordAsync(int artistId, int recordId);
        Task<IEnumerable<ArtistRecordDto>> GetByRecordIdAsync(int recordId);
        Task<IEnumerable<ArtistRecordDto>> GetByArtistIdAsync(int artistId);
        Task<ArtistRecordDto> CreateArtistRecordAsync(CreateArtistRecordDto createDto);
        Task<ArtistRecordDto> UpdateArtistRecordAsync(int id, UpdateArtistRecordDto updateDto);
        Task<bool> DeleteArtistRecordAsync(int id);
        Task<bool> RemoveArtistFromRecordAsync(int artistId, int recordId);
    }
}
