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
        private DbSet<IProduct> _entities;
        public ProductService(DbSet<IProduct> entities) {
            _dataContext = new DataContext();
            _entities = entities;
        }
        public async Task<bool> ProductExists(IProduct productEntity)
        {
            var product = await _entities.FirstOrDefaultAsync(x => x.Id == productEntity.Id);
            return product == null;
        }
        public async Task UpdateAsync(IProduct productEntity)
        {
            var product = await _entities.FirstOrDefaultAsync(x => x.Id == productEntity.Id);
            if(product == null)
                _entities.Update(productEntity);
            await _dataContext.SaveChangesAsync();
        }
        public IProduct GetProduct(int id)
        {
            var product = _entities.FirstOrDefault(x => x.Id == id);
            if (product != null) return product;
            throw new ArgumentNullException(nameof(product));
        }
        public async Task AddProductAsync(IProduct product)
        {
            await _entities.AddAsync(product);
            await _dataContext.SaveChangesAsync();
        }
        public async Task EditProductAsync(IProduct product)
        {
            _dataContext.Update(product);
            await _dataContext.SaveChangesAsync();
        }
        public async Task DeleteProductAsync(IProduct product)
        {
            _dataContext.Remove(product);
            await _dataContext.SaveChangesAsync();
        }
        public async Task DeleteProductAsync(int index)
        {
            var product = await _entities.FirstOrDefaultAsync(x => x.Id == index);
            if (product != null) _dataContext.Remove(product);
            else throw new ArgumentNullException(nameof(product));
            await _dataContext.SaveChangesAsync();
        }
        public IProduct GetLast()
        {
            return _entities.FirstOrDefault(x => x.Id == _entities.Count());
        }
        public IEnumerable<IProduct> GetRange(IEnumerable<int> IDs)
        {
            foreach (var id in IDs)
            {
                yield return _entities.FirstOrDefault(x => x.Id == id);
            }
        }
        public IEnumerable<IProduct> GetAll() {
            return _entities;
        }
    }
}
