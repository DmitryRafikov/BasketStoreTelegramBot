using BasketStoreTelegramBot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Services.CatalogProduct
{
    interface ICatalogProductService
    {
        CatalogProductEntity GetProduct(int id);
        List<CatalogProductEntity> GetAllProducts();
        CatalogProductEntity GetLast();
        Task AddProductAsync(CatalogProductEntity product);
        Task EditProductAsync(CatalogProductEntity product);
        Task DeleteProductAsync(CatalogProductEntity product);
        Task DeleteProductAsync(int index);
    }
}
