using BasketStoreTelegramBot.Entities;
using BasketStoreTelegramBot.Entities.Products;
using BasketStoreTelegramBot.Models;
using BasketStoreTelegramBot.Services.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Services.Order
{
    class OrderService : IOrderService
    {
        private DataContext _dataContext;
        public OrderService() {
            _dataContext = new DataContext();
        }
        public async Task AddOrderAsync(OrderEntity order)
        {
            await _dataContext.Orders.AddAsync(order);
            await _dataContext.SaveChangesAsync();
        }
        public async Task EditOrderAsync(OrderEntity order)
        {
            int id = order.Id;
            var orderToEdit = _dataContext.Orders.FirstOrDefault(x => x.Id == id);
            if (orderToEdit != null) {
                _dataContext.Orders.Remove(orderToEdit);
                await _dataContext.Orders.AddAsync(order);
                return;
            }
            throw new ArgumentException("Пользователь не найден");

        }
        public OrderEntity GetOrderById(int id)
        {
            var order = _dataContext.Orders.FirstOrDefault(x => x.Id == id);
            if (order != null) return order;
            throw new ArgumentOutOfRangeException();
        }
        public async Task AddProductToOrderAsync(ProductEntity product, int orderId)
        {
            var order = await _dataContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            if (order != null) {
                if (order.ProductIDs != null || order.ProductIDs != string.Empty)
                    order.ProductIDs += product.Id.ToString();
                else 
                    order.ProductIDs = product.Id.ToString();
                return;
            }
            throw new ArgumentOutOfRangeException(nameof(orderId));
        }
        public async Task RemoveProductInOrderAsync(ProductEntity product, int orderId)
        {
            var order = await _dataContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            if (order != null) {
                if (order.ProductIDs != null || order.ProductIDs != string.Empty) {
                    string id = product.Id.ToString();
                    int startIndex = order.ProductIDs.IndexOf(id);
                    order.ProductIDs.Remove(startIndex);
                    return;
                }
                throw new ArgumentException(nameof(orderId));
            }
            throw new ArgumentOutOfRangeException(nameof(orderId));
        }

        public OrderEntity GetLast()
        {
            return _dataContext.Orders.Last();
        }
    }
}
