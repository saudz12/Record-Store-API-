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
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;

        public InventoryService(IInventoryRepository inventoryRepository, IMapper mapper)
        {
            _inventoryRepository = inventoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InventoryDto>> GetAllInventoryAsync()
        {
            var inventory = await _inventoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<InventoryDto>>(inventory);
        }

        public async Task<InventoryDto> GetInventoryByIdAsync(int id)
        {
            var inventory = await _inventoryRepository.GetByIdAsync(id);
            return _mapper.Map<InventoryDto>(inventory);
        }

        public async Task<InventoryDto> GetInventoryByRecordIdAsync(int recordId)
        {
            var inventory = await _inventoryRepository.GetByRecordIdAsync(recordId);
            return _mapper.Map<InventoryDto>(inventory);
        }

        public async Task<InventoryDto> CreateInventoryAsync(CreateInventoryDto createInventoryDto)
        {
            var inventory = _mapper.Map<Inventory>(createInventoryDto);
            var createdInventory = await _inventoryRepository.CreateAsync(inventory);
            return _mapper.Map<InventoryDto>(createdInventory);
        }

        public async Task<InventoryDto> UpdateInventoryAsync(int id, UpdateInventoryDto updateInventoryDto)
        {
            var existingInventory = await _inventoryRepository.GetByIdAsync(id);
            if (existingInventory == null) throw new RecordNotFoundException(id);

            _mapper.Map(updateInventoryDto, existingInventory);
            var updatedInventory = await _inventoryRepository.UpdateAsync(existingInventory);
            return _mapper.Map<InventoryDto>(updatedInventory);
        }

        public async Task<bool> DeleteInventoryAsync(int id)
        {
            return await _inventoryRepository.DeleteAsync(id);
        }

        public async Task<bool> UpdateStockAsync(int recordId, int quantity)
        {
            return await _inventoryRepository.UpdateStockAsync(recordId, quantity);
        }
    }
}
