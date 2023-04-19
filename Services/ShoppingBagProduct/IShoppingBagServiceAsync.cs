using BasketStoreTelegramBot.Entities;
using BasketStoreTelegramBot.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Services.ShoppingBagProduct
{
    internal interface IShoppingBagServiceAsync
    {
        public Task AddRangeAsync(UserEntity user, IEnumerable<ProductEntity> products);
        public Task AddAsync(UserEntity user, ProductEntity product);
        public Task RemoveAsync(Entities.ShoppingBagProduct shoppingBagProduct);

    }
}
