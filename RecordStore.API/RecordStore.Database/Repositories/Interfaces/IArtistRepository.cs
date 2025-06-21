using RecordStore.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Database.Repositories.Interfaces
{
    public interface IArtistRepository
    {
        Task<IEnumerable<Artist>> GetAllAsync();
        Task<Artist> GetByIdAsync(int id);
        Task<Artist> CreateAsync(Artist artist);
        Task<Artist> UpdateAsync(Artist artist);
        Task<bool> DeleteAsync(int id);
    }
}
