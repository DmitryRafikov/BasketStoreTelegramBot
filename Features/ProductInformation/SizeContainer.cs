using System;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
 
namespace BasketStoreTelegramBot.Features.ProductInformation
{
    [Serializable]
    public class SizeContainer:ISizeContainer
    {
        [JsonProperty("height")] public int? Height { get; set; }
        [JsonProperty("length")] public int? Length { get; set; }
        [JsonProperty("width")] public int? Width { get; set; }
        [JsonProperty("diameter")] public int? Diameter { get; set; }
        [JsonProperty("handles")] public int? AdditionalSize { get; set; }
        [JsonProperty("cost")] public int? Cost { get; set; }
        public bool HasValue
        {
            get
            {
                return Height.HasValue &&
                                     Length.HasValue && Width.HasValue ||
                                    Height.HasValue && Diameter.HasValue;
            }
        }
        
        public override string ToString()
        {
            return ConvertSize("Высота: ", Height)
                + ConvertSize("Длина: ", Length)
                + ConvertSize("Ширина: ", Width)
                + ConvertSize("Диаметр: ", Diameter)
                + ConvertSize("Длина ручек: ", AdditionalSize);
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType()) return false;
            SizeContainer sizes = obj as SizeContainer;
            return this.Height == sizes.Height && this.Length == sizes.Length && this.Width == sizes.Width
                && this.Diameter == sizes.Diameter && this.AdditionalSize == sizes.AdditionalSize;
        }
        private string ConvertSize(string separator, int? size) => size.HasValue ? separator + size.ToString() + "\n" : string.Empty;
    }
}
