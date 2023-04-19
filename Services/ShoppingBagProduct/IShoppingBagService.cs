using BasketStoreTelegramBot.Entities;
using BasketStoreTelegramBot.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Services.ShoppingBagProduct
{
    internal interface IShoppingBagService
    {
        void AddRange(UserEntity user, IEnumerable<ProductEntity> products);
        void Add(UserEntity user, ProductEntity product);
        void Add(int chatID, ProductEntity product);
        void Remove(Entities.ShoppingBagProduct shoppingBagProduct);
        void Update(Entities.ShoppingBagProduct shoppingBagProduct);
        Entities.ShoppingBagProduct GetLastEntry(int chatID);
        ProductEntity GetLastProduct(int chatID);
        IEnumerable<Entities.ShoppingBagProduct> GetRange(int chatID);
    }
}
