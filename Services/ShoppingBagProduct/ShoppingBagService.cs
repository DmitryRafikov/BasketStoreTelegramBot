using BasketStoreTelegramBot.Entities;
using BasketStoreTelegramBot.Entities.Products;
using BasketStoreTelegramBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Services.ShoppingBagProduct
{
    internal class ShoppingBagService : IShoppingBagService, IShoppingBagServiceAsync
    {
        DataContext _dataContext;
        public ShoppingBagService() {
            _dataContext = new DataContext();
        }
        public void Add(UserEntity user, ProductEntity product)
        {
            _dataContext.ShoppingBagProducts.Add(new Entities.ShoppingBagProduct() { 
                ChatID = user.ID.ToString(),
                ProductID = product.Id.ToString()
            });
            _dataContext.SaveChanges();
        }
        public void Add(int chatID, ProductEntity product)
        {
            _dataContext.ShoppingBagProducts.Add(new Entities.ShoppingBagProduct()
            {
                ChatID = chatID.ToString(),
                ProductID = product.Id.ToString()
            });
            _dataContext.SaveChanges();
        }
        public async Task AddAsync(UserEntity user, ProductEntity product)
        {
            await _dataContext.ShoppingBagProducts.AddAsync(new Entities.ShoppingBagProduct()
            {
                ChatID = user.ID.ToString(),
                ProductID = product.Id.ToString()
            });
            await _dataContext.SaveChangesAsync();
        }
        public void AddRange(UserEntity user, IEnumerable<ProductEntity> products)
        {
            var shoppingBagProducts = new List<Entities.ShoppingBagProduct>();
            foreach (ProductEntity product in products)
            {
                shoppingBagProducts.Add(new Entities.ShoppingBagProduct() {
                    ChatID = user.ID.ToString(),
                    ProductID = product.Id.ToString()
                });
            }
            _dataContext.ShoppingBagProducts.AddRange(shoppingBagProducts.ToArray());
            _dataContext.SaveChanges();
        }
        public async Task AddRangeAsync(UserEntity user, IEnumerable<ProductEntity> products)
        {

            var shoppingBagProducts = new List<Entities.ShoppingBagProduct>();
            foreach (ProductEntity product in products)
            {
                shoppingBagProducts.Add(new Entities.ShoppingBagProduct()
                {
                    ChatID = user.ID.ToString(),
                    ProductID = product.Id.ToString()
                });
            }
            await _dataContext.ShoppingBagProducts.AddRangeAsync(shoppingBagProducts.ToArray());
            await _dataContext.SaveChangesAsync();
        }
        public void Remove(Entities.ShoppingBagProduct shoppingBagProduct)
        {
            var products = _dataContext.ShoppingBagProducts.ToList();
            if(products.Contains(shoppingBagProduct))
                _dataContext.ShoppingBagProducts.Remove(shoppingBagProduct);
            _dataContext.SaveChanges();
        }
        public async Task RemoveAsync(Entities.ShoppingBagProduct shoppingBagProduct)
        {
            _dataContext.ShoppingBagProducts.Remove(shoppingBagProduct);
            await _dataContext.SaveChangesAsync();
        }
        public void Update(Entities.ShoppingBagProduct shoppingBagProduct)
        {
            _dataContext.ShoppingBagProducts.Update(shoppingBagProduct);
            _dataContext.SaveChanges();
        }
        public Entities.ShoppingBagProduct GetLastEntry(int chatID)
        {
            return _dataContext.ShoppingBagProducts.OrderBy(x => x.ID)
                                                   .ToList()
                                                   .Last(n => n.ChatID == chatID.ToString());           
        }
        public ProductEntity GetLastProduct(int chatID)
        {
            var id = chatID.ToString();
            var lastData= _dataContext.ShoppingBagProducts.OrderBy(x=>x.ChatID).ToList().Last(n => n.ChatID == id);
            return _dataContext.Products.OrderBy(x=>x.Id).ToList().First(n => n.Id == Convert.ToInt32(lastData.ProductID));
        }
        public IEnumerable<Entities.ShoppingBagProduct> GetRange(int chatID)
        {
            return _dataContext.ShoppingBagProducts.Where(n => n.ChatID == chatID.ToString());
        }
    }
}
