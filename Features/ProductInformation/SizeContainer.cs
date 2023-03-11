using System;
using Newtonsoft.Json;
using System.Collections.Generic;
 
namespace BasketStoreTelegramBot.Features.ProductInformation
{
    [Serializable]
    class SizeContainer
    {
        private BasketsGeometryTypes _type;
        [JsonProperty("height")] public int? Height { get; set; }
        [JsonProperty("length")] public int? Length { get; set; }
        [JsonProperty("width")] public int? Width { get; set; }
        [JsonProperty("diameter")] public int? Diameter { get; set; }
        [JsonProperty("handles")] public int? Handles { get; set; }
        public SizeContainer()
        {

        }
        public SizeContainer(string text, BasketsGeometryTypes type)
        {
            _type = type;
            if (ValidateInput(text))
            {
                var sizes = InsertValues(text);
                switch (_type)
                {
                    case BasketsGeometryTypes.CIRCLE:
                        Height = sizes[0];
                        Diameter = sizes[1];
                        return;
                    case BasketsGeometryTypes.RECTANGLE:
                        Height = sizes[0];
                        Length = sizes[1];
                        Width = sizes[2];
                        return;
                }
            }
        }
        private bool ValidateInput(string text)
        {
            int counter = 0;
            int maxCounterValue = _type == BasketsGeometryTypes.CIRCLE ? 1 : 2;
            for (int i = 0; i < text.Length; i++)
            {
                if (!char.IsDigit(text[i]))
                    counter++;
                if (counter > maxCounterValue)
                    return false;
            }
            return counter >= maxCounterValue;
        }
        private List<int> InsertValues(string text)
        {
            string str = string.Empty;
            var sizesCounter = new List<int>();
            int maxCounterValue = _type == BasketsGeometryTypes.CIRCLE ? 2 : 3;
            for (int i = 0; i < text.Length; i++)
            {
                if (char.IsDigit(text[i]))
                {
                    str += text[i];
                }
                else if (sizesCounter.Count <= maxCounterValue)
                {
                    sizesCounter.Add(Convert.ToInt32(str));
                    str = string.Empty;
                }
                else throw new ArgumentException("Строка указана в неверном формате");
            }
            sizesCounter.Add(Convert.ToInt32(str));
            return sizesCounter;
        }
        public bool HasValue
        {
            get
            {
                return Height.HasValue &&
                                     Length.HasValue && Width.HasValue ||
                                    Height.HasValue && Diameter.HasValue;
            }
        }

        public string InsertStringSizes()
        {
            char separator = 'x';
            return (TryGetSize(Height, separator)
                + TryGetSize(Length, separator)
                + TryGetSize(Width, separator)
                + TryGetSize(Diameter, separator)
                + TryGetSize(Handles, separator)).Remove(0, 1);
        }
        public override string ToString()
        {
            return ConvertSize("Высота: ", Height)
                + ConvertSize("Длина: ", Length)
                + ConvertSize("Ширина: ", Width)
                + ConvertSize("Диаметр: ", Diameter)
                + ConvertSize("Длина ручек: ", Handles);
        }
        private string TryGetSize(int? size, char? separator) => size.HasValue ? separator + size.ToString() : string.Empty;
        private string ConvertSize(string separator, int? size) => size.HasValue ? separator + size.ToString() + "\n" : string.Empty;
    }
}
