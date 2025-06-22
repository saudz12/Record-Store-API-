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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IInventoryRepository inventoryRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _inventoryRepository = inventoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            // Validate inventory availability
            foreach (var item in createOrderDto.OrderRecords)
            {
                var inventory = await _inventoryRepository.GetByRecordIdAsync(item.RecordId);
                if (inventory == null || inventory.Quantity < item.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for record ID {item.RecordId}");
                }
            }

            var order = new Order
            {
                UserId = createOrderDto.UserId,
                PlacementDate = DateTime.UtcNow,
                OrderRecords = createOrderDto.OrderRecords.Select(or => new OrderRecord
                {
                    RecordId = or.RecordId,
                    Quantity = or.Quantity
                }).ToList()
            };

            var createdOrder = await _orderRepository.CreateAsync(order);

            // Update inventory
            foreach (var item in createOrderDto.OrderRecords)
            {
                var inventory = await _inventoryRepository.GetByRecordIdAsync(item.RecordId);
                inventory.Quantity -= item.Quantity;
                await _inventoryRepository.UpdateAsync(inventory);
            }

            return _mapper.Map<OrderDto>(createdOrder);
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            return await _orderRepository.DeleteAsync(id);
        }
    }
}
