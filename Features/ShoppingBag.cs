using BasketStoreTelegramBot.Entities;
using BasketStoreTelegramBot.Others;
using BasketStoreTelegramBot.Services.Order;
using BasketStoreTelegramBot.Services.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace BasketStoreTelegramBot.Features
{

    class ShoppingBag
    {
        private static readonly Lazy<ShoppingBag> bag = new Lazy<ShoppingBag>(() => new ShoppingBag());
        public static ShoppingBag Instance { get { return bag.Value; } }
        public ProductEntity CurrentProduct { get => _currentProduct; }
        public List<ProductEntity> Products { get => _products; }
        private List<ProductEntity> _products;
        private ProductEntity _currentProduct;
        private IProductService _productService = ProductService.Instance;
        private IOrderService _orderService = OrderService.Instance;
        private ShoppingBag()
        {
            _products = new List<ProductEntity>();
        }
        public void CreateProduct()
        {
            int productId = _productService.GetLast().Id + _products.Count() + 1;
            _currentProduct = new ProductEntity()
            { 
                Id = productId
            };
            _products.Add(_currentProduct);
        }
        public void RemoveProductByIndex(int index) 
        {
            _products.RemoveAt(index);
        }
        public void AddProductInBag(ProductEntity product) {
            _products.Add(product);
        }
        public async Task EndProductSerializationAsync() {
            await _productService.AddProductAsync(_currentProduct);
        }
        public async Task BuildOrderAsync(MessageEvent data) 
        {
            var order = new OrderEntity()
            {
                Id = _orderService.GetLast().Id,
                ChatID = data.Id,
                ProductIDs = ListConverter.ToString(_products.Select(x => x.Id.ToString()).ToList(), ", "),
                Cost = (int)_products.Sum(x => x.Cost != null ?
                                                        x.Cost :
                                                        ProductExtension.CalculateCost(x))
            };
            await _orderService.AddOrderAsync(order);
        }

    }
}
