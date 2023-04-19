using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Features.ProductInformation
{
    public interface ISizeContainer
    {
        int? Height { get; set; }
        int? Length { get; set; }
        int? Width { get; set; }
        int? Diameter { get; set; }
        int? AdditionalSize { get; set; }
    }
}
