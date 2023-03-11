using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Entities
{
    class ProductEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string? Specifics { get; set; }
        public int? Height { get; set; }
        public int? Width { get; set; }
        public int? Length { get; set; }
        public int? Diameter { get; set; }
        public int? AddidionalSize { get; set; }
        public int? Cost { get; set; }
        public string? BottomType { get; set; }
        public int? MinCost { get; set; }   

    }
}
