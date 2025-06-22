using AutoMapper;
using RecordStore.Core.Dtos;
using RecordStore.Database.Entities;
using RecordStore.Database.Repositories.Interfaces;
using RecordStore.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecordStore.Core.Services.Interfaces;

namespace RecordStore.Core.Services
{
    public class OrderRecordService : IOrderRecordService
    {
        private readonly IOrderRecordRepository _orderRecordRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;

        public OrderRecordService(IOrderRecordRepository orderRecordRepository, IInventoryRepository inventoryRepository, IMapper mapper)
        {
            _orderRecordRepository = orderRecordRepository;
            _inventoryRepository = inventoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderRecordDto>> GetAllOrderRecordsAsync()
        {
            var orderRecords = await _orderRecordRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderRecordDto>>(orderRecords);
        }

        public async Task<OrderRecordDto> GetOrderRecordByIdAsync(int id)
        {
            var orderRecord = await _orderRecordRepository.GetByIdAsync(id);
            return _mapper.Map<OrderRecordDto>(orderRecord);
        }

        public async Task<OrderRecordDto> GetByOrderAndRecordAsync(int orderId, int recordId)
        {
            var orderRecord = await _orderRecordRepository.GetByOrderAndRecordAsync(orderId, recordId);
            return _mapper.Map<OrderRecordDto>(orderRecord);
        }

        public async Task<IEnumerable<OrderRecordDto>> GetByOrderIdAsync(int orderId)
        {
            var orderRecords = await _orderRecordRepository.GetByOrderIdAsync(orderId);
            return _mapper.Map<IEnumerable<OrderRecordDto>>(orderRecords);
        }

        public async Task<IEnumerable<OrderRecordDto>> GetByRecordIdAsync(int recordId)
        {
            var orderRecords = await _orderRecordRepository.GetByRecordIdAsync(recordId);
            return _mapper.Map<IEnumerable<OrderRecordDto>>(orderRecords);
        }

        public async Task<OrderRecordDto> CreateOrderRecordAsync(CreateOrderRecordDto createDto)
        {
            // check if item already exists in order
            var existing = await _orderRecordRepository.GetByOrderAndRecordAsync(createDto.OrderId, createDto.RecordId);
            if (existing != null)
            {
                throw new InvalidOperationException("Record is already in this order");
            }

            // check inventory availability
            var inventory = await _inventoryRepository.GetByRecordIdAsync(createDto.RecordId);
            if (inventory == null || inventory.Quantity < createDto.Quantity)
            {
                throw new InvalidOperationException("Insufficient stock");
            }

            var orderRecord = _mapper.Map<OrderRecord>(createDto);
            var created = await _orderRecordRepository.CreateAsync(orderRecord);

            // update inventory
            inventory.Quantity -= createDto.Quantity;
            await _inventoryRepository.UpdateAsync(inventory);

            return _mapper.Map<OrderRecordDto>(created);
        }

        public async Task<OrderRecordDto> UpdateOrderRecordAsync(int id, UpdateOrderRecordDto updateDto)
        {
            var existing = await _orderRecordRepository.GetByIdAsync(id);
            if (existing == null) return null;

            if (updateDto.Quantity.HasValue)
            {
                var inventory = await _inventoryRepository.GetByRecordIdAsync(existing.RecordId);
                var quantityDifference = updateDto.Quantity.Value - existing.Quantity;

                if (quantityDifference > 0 && inventory.Quantity < quantityDifference)
                {
                    throw new InvalidOperationException("Insufficient stock for quantity increase");
                }

                // Update inventory
                inventory.Quantity -= quantityDifference;
                await _inventoryRepository.UpdateAsync(inventory);
            }

            _mapper.Map(updateDto, existing);
            var updated = await _orderRecordRepository.UpdateAsync(existing);
            return _mapper.Map<OrderRecordDto>(updated);
        }

        public async Task<bool> DeleteOrderRecordAsync(int id)
        {
            var orderRecord = await _orderRecordRepository.GetByIdAsync(id);
            if (orderRecord == null) return false;

            // return stock to inventory
            var inventory = await _inventoryRepository.GetByRecordIdAsync(orderRecord.RecordId);
            if (inventory != null)
            {
                inventory.Quantity += orderRecord.Quantity;
                await _inventoryRepository.UpdateAsync(inventory);
            }

            return await _orderRecordRepository.DeleteAsync(id);
        }

        public async Task<bool> RemoveRecordFromOrderAsync(int orderId, int recordId)
        {
            var orderRecord = await _orderRecordRepository.GetByOrderAndRecordAsync(orderId, recordId);
            if (orderRecord == null) return false;

            // return stock to inventory
            var inventory = await _inventoryRepository.GetByRecordIdAsync(recordId);
            if (inventory != null)
            {
                inventory.Quantity += orderRecord.Quantity;
                await _inventoryRepository.UpdateAsync(inventory);
            }

            return await _orderRecordRepository.DeleteByOrderAndRecordAsync(orderId, recordId);
        }
    }
}
