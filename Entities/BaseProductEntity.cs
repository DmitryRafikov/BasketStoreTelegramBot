using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Entities
{
    abstract class BaseProductEntity
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public string? Specifics { get; set; }
        public int? Height { get; set; }
        public int? Width { get; set; }
        public int? Length { get; set; }
        public int? Diameter { get; set; }
        public int? Handles { get; set; }
        public int? Cost { get; set; }
    }
}
