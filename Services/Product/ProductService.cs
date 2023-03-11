using BasketStoreTelegramBot.Entities;
using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.Features.ProductInformation;
using BasketStoreTelegramBot.Models;
using BasketStoreTelegramBot.Others;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Services.Product
{
    class ProductService : IProductService
    {
        private static Lazy<ProductService> _productService = new Lazy<ProductService>(() => new ProductService());
        public static ProductService Instance { get => _productService.Value; }

        private DataContext _dataContext = DataContext.Instance;
        public ProductEntity GetProduct(int id)
        {
            var product = _dataContext.Products.FirstOrDefault(x => x.Id == id);
            if (product != null) return product;
            throw new ArgumentNullException(nameof(product));
        }
        public async Task AddProductAsync(ProductEntity product)
        {
            if (product.Id < _dataContext.Products.Count())
                product.Id = _dataContext.Products.Count() + 1;
            await _dataContext.Products.AddAsync(product);
            await _dataContext.SaveChangesAsync();
        }
        public async Task EditProductAsync(ProductEntity product)
        {
            _dataContext.Update(product);
            await _dataContext.SaveChangesAsync();
        }
        public async Task DeleteProductAsync(ProductEntity product)
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
        public ProductEntity GetLast()
        {
            return _dataContext.Products.FirstOrDefault(x => x.Id == _dataContext.Products.Count());
        }
    }
}
