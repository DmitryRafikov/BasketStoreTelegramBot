using BasketStoreTelegramBot.Entities;
using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.Features.ProductInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Others
{
    class ProductExtension
    {
        public BasketsGeometryTypes GeometryType { get; set; }
        public static SizeContainer Size { get; set; }
        public static string ToString(ProductEntity product)
        {
            return product.Name +
                "\nЦвет: " + product.Color +
                "\n" + Size.ToString() +
                "\nОсобенности: " + product.Specifics +
                "\nИтоговая стоимость товара: " + product.Cost;
        }
        public static int CalculateCost(ProductEntity product)
        {
            return 0;
        }
    }
}
