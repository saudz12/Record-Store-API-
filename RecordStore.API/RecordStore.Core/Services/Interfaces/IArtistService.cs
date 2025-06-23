using RecordStore.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Core.Services.Interfaces
{
    public interface IArtistService
    {
        Task<IEnumerable<ArtistDto>> GetAllArtistsAsync();
        Task<ArtistDto> GetArtistByIdAsync(int id);
        Task<ArtistDto> CreateArtistAsync(CreateArtistDto createArtistDto);
        Task<ArtistDto> UpdateArtistAsync(int id, UpdateArtistDto updateArtistDto);
        Task<bool> DeleteArtistAsync(int id);
    }
}
