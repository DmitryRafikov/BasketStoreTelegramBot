using BasketStoreTelegramBot.Entities;
using BasketStoreTelegramBot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Services.CatalogProduct
{
    class CatalogProductService : ICatalogProductService
    {
        private static Lazy<CatalogProductService> _productService = new Lazy<CatalogProductService>(() => new CatalogProductService());
        public static CatalogProductService Instance { get => _productService.Value; }

        private DataContext _dataContext = DataContext.Instance;
        public CatalogProductEntity GetProduct(int id)
        {
            var product = _dataContext.CatalogProducts.FirstOrDefault(x => x.Id == id);
            if (product != null) return product;
            throw new ArgumentNullException(nameof(product));
        }
        public async Task AddProductAsync(CatalogProductEntity product)
        {
            if (product.Id < _dataContext.CatalogProducts.Count())
                product.Id = _dataContext.CatalogProducts.Count() + 1;
            await _dataContext.CatalogProducts.AddAsync(product);
            await _dataContext.SaveChangesAsync();
        }
        public async Task EditProductAsync(CatalogProductEntity product)
        {
            _dataContext.Update(product);
            await _dataContext.SaveChangesAsync();
        }
        public async Task DeleteProductAsync(CatalogProductEntity product)
        {
            _dataContext.Remove(product);
            await _dataContext.SaveChangesAsync();
        }
        public async Task DeleteProductAsync(int index)
        {
            var product = await _dataContext.Products.FirstOrDefaultAsync(x => x.Id == index);
            if (product != null) _dataContext.Remove(product);
            else throw new ArgumentNullException(nameof(product));
            await _dataContext.SaveChangesAsync();
        }
        public CatalogProductEntity GetLast()
        {
            return _dataContext.CatalogProducts.FirstOrDefault(x => x.Id == _dataContext.Products.Count());
        }

        public List<CatalogProductEntity> GetAllProducts()
        {
            return _dataContext.CatalogProducts.Select(x=>x).ToList();
        }
    }
}
