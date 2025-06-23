using RecordStore.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Database.Repositories.Interfaces
{
    public interface IArtistRecordRepository
    {
        Task<IEnumerable<ArtistRecord>> GetAllAsync();
        Task<ArtistRecord> GetByIdAsync(int id);
        Task<ArtistRecord> GetByArtistAndRecordAsync(int artistId, int recordId);
        Task<IEnumerable<ArtistRecord>> GetByRecordIdAsync(int recordId);
        Task<IEnumerable<ArtistRecord>> GetByArtistIdAsync(int artistId);
        Task<ArtistRecord> CreateAsync(ArtistRecord artistRecord);
        Task<ArtistRecord> UpdateAsync(ArtistRecord artistRecord);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByArtistAndRecordAsync(int artistId, int recordId);
    }
}
