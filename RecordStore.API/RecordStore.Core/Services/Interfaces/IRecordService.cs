using RecordStore.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Core.Services.Interfaces
{
    public interface IRecordService
    {
        Task<IEnumerable<RecordDto>> GetAllRecordsAsync();
        Task<RecordDto> GetRecordByIdAsync(int id);
        Task<RecordDto> CreateRecordAsync(CreateRecordDto createRecordDto);
        Task<RecordDto> UpdateRecordAsync(int id, UpdateRecordDto updateRecordDto);
        Task<bool> DeleteRecordAsync(int id);
        Task<IEnumerable<RecordDto>> GetRecordsByGenreAsync(string genre);
        Task<IEnumerable<RecordDto>> SearchRecordsAsync(string searchTerm);
    }
}
