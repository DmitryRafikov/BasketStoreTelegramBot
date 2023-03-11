using BasketStoreTelegramBot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Services.Order
{
    interface IOrderService
    {
        Task AddProductToOrderAsync(ProductEntity product, int orderId);
        Task RemoveProductInOrderAsync(ProductEntity product, int orderId);
        Task AddOrderAsync(OrderEntity order);
        Task EditOrderAsync(OrderEntity order);
        OrderEntity GetOrderById(int id);
        OrderEntity GetLast();
    }
}
