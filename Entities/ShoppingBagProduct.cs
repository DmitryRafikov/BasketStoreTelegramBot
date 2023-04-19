using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Entities
{
    class ShoppingBagProduct
    {
        public string ChatID {get;set;}
        public string ProductID {get;set;}
        public bool Added { get;set;}
        public override bool Equals(object obj)
        {
            if(obj.GetType() != this.GetType())
                return false;
            ShoppingBagProduct other = (ShoppingBagProduct)obj;
            return this.ChatID == other.ChatID &&
                    this.ProductID == other.ProductID;
        }
    }
}
