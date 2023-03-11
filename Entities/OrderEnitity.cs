using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Entities
{
    class OrderEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string ChatID { get; set; }
        public string ProductIDs { get; set; }
        public int Cost { get; set; }
    }
}
