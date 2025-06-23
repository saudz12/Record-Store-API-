using AutoMapper;
using RecordStore.Core.Dtos;
using RecordStore.Core.Services.Interfaces;
using RecordStore.Database.Entities;
using RecordStore.Database.Repositories.Interfaces;
using RecordStore.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Core.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IMapper _mapper;

        public ArtistService(IArtistRepository artistRepository, IMapper mapper)
        {
            _artistRepository = artistRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArtistDto>> GetAllArtistsAsync()
        {
            var artists = await _artistRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ArtistDto>>(artists);
        }

        public async Task<ArtistDto> GetArtistByIdAsync(int id)
        {
            var artist = await _artistRepository.GetByIdAsync(id);
            return _mapper.Map<ArtistDto>(artist);
        }

        public async Task<ArtistDto> CreateArtistAsync(CreateArtistDto createArtistDto)
        {
            var artist = _mapper.Map<Artist>(createArtistDto);
            var createdArtist = await _artistRepository.CreateAsync(artist);
            return _mapper.Map<ArtistDto>(createdArtist);
        }

        public async Task<ArtistDto> UpdateArtistAsync(int id, UpdateArtistDto updateArtistDto)
        {
            var existingArtist = await _artistRepository.GetByIdAsync(id);
            if (existingArtist == null)
                throw new ArtistNotFoundException(id);

            _mapper.Map(updateArtistDto, existingArtist);
            var updatedArtist = await _artistRepository.UpdateAsync(existingArtist);
            return _mapper.Map<ArtistDto>(updatedArtist);
        }

        public async Task<bool> DeleteArtistAsync(int id)
        {
            return await _artistRepository.DeleteAsync(id);
        }
    }
}
