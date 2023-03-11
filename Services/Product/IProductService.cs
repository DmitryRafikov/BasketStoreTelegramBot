using BasketStoreTelegramBot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Services.Product
{
    interface IProductService
    {
        ProductEntity GetProduct(int id);
        ProductEntity GetLast();
        Task AddProductAsync(ProductEntity product);
        Task EditProductAsync(ProductEntity product);
        Task DeleteProductAsync(ProductEntity product);
        Task DeleteProductAsync(int index);

    }
}
