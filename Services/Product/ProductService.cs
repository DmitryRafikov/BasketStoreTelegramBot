using BasketStoreTelegramBot.Entities.Products;
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
    class ProductService
    {

        private DataContext _dataContext;
        public ProductService() {
            _dataContext = new DataContext();
        }
        public bool ProductExists(ProductEntity productEntity)
        {
            var product = _dataContext.Products.FirstOrDefault(x => x.Id == productEntity.Id);
            return product != null;
        }
        public async Task UpdateAsync(ProductEntity productEntity)
        {
            var product = await _dataContext.Products.FirstOrDefaultAsync(x => x.Id == productEntity.Id);
            if(product != null)
                _dataContext.Products.Update(product.Combine(productEntity));
            Console.WriteLine(_dataContext.SaveChanges());
            await _dataContext.SaveChangesAsync();
        }
        public ProductEntity GetProduct(int id)
        {
            var product = _dataContext.Products.FirstOrDefault(x => x.Id == id);
            if (product != null) return product;
            throw new ArgumentNullException(nameof(product));
        }
        public async Task AddProductAsync(ProductEntity product)
        {
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
            _dataContext.SaveChanges();
        }
        public ProductEntity GetLast()
        {
            return _dataContext.Products.Where(x => x.IsCatalogProduct == null).OrderBy(x => x.Id).Last();
        }
        public IEnumerable<ProductEntity> GetRange(IEnumerable<int> IDs)
        {
            foreach (var id in IDs)
            {
                yield return _dataContext.Products.FirstOrDefault(x => x.Id == id);
            }
        }
        public IEnumerable<ProductEntity> GetAll() {
            return _dataContext.Products;
        }
    }
}
