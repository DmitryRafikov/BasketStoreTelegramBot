using BasketStoreTelegramBot.Entities;
using BasketStoreTelegramBot.Entities.Products;
using BasketStoreTelegramBot.Others;
using BasketStoreTelegramBot.Services.Order;
using BasketStoreTelegramBot.Services.Product;
using BasketStoreTelegramBot.Services.ShoppingBagProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Features
{

    class ShoppingBag
    {
        private ProductService _productService;
        private IOrderService _orderService;
        private IShoppingBagService _shoppingBagService;
        public ShoppingBag()
        {
            _productService = new ProductService();
            _orderService = new OrderService();
            _shoppingBagService = new ShoppingBagService();
        }
        public void CreateProduct(int chatID)
        { 
            _productService.AddProductAsync(new ProductEntity());
            _shoppingBagService.Add(chatID, _productService.GetLast());
        }
        public ProductEntity CurrentProduct(int chatID)
        {
            return _shoppingBagService.GetLastProduct(chatID);
        }
        public IEnumerable<ProductEntity> GetAddedProducts(int chatID)
        {
            var products = _shoppingBagService.GetRange(chatID).Where(n => n.Added == true);
            return _productService.GetRange(products.Select(n => Convert.ToInt32(n.ProductID)));
        }
        public void RemoveProduct(int chatID, ProductEntity product) 
        {
            var recordToRemove = _shoppingBagService.GetRange(chatID)
                                                    .ToList()
                                                    .First(x => x.ProductID == product.Id.ToString());
            _shoppingBagService.Remove(recordToRemove);

        }
        public void AddProductInBag(int chatID) {
            var last = _shoppingBagService.GetLastEntry(chatID);
            last.Added = true;
            _shoppingBagService.Update(last);
        }
        public void AddProductInBag(int chatID, ProductEntity product)
        {
            var last = _shoppingBagService.GetLastEntry(chatID);
            last.Added = true;
            _shoppingBagService.Update(last);
        }
        public async Task UpdateInfo(ProductEntity currentProduct) {
            if (!_productService.ProductExists(currentProduct))
                await _productService.AddProductAsync(currentProduct);
            await _productService.UpdateAsync(currentProduct);
            
        }
        public async Task BuildOrderAsync(int chatID) 
        {
            var products = GetAddedProducts(chatID);
            var order = new OrderEntity()
            {
                ChatID = chatID.ToString(),
                ProductIDs = ListConverter.ToString(products.Select(x => x.Id.ToString()).ToList(), ", "),
                Cost = (int)products.Sum(x => x.Cost)
            };
            await _orderService.AddOrderAsync(order);
        }
    }
}
