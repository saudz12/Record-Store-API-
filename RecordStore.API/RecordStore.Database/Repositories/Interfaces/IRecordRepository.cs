using RecordStore.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Database.Repositories.Interfaces
{
    public interface IRecordRepository
    {
        Task<IEnumerable<Record>> GetAllAsync();
        Task<Record> GetByIdAsync(int id);
        Task<Record> CreateAsync(Record record);
        Task<Record> UpdateAsync(Record record);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Record>> GetByGenreAsync(string genre);
        Task<IEnumerable<Record>> SearchAsync(string searchTerm);
    }
}
