using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Features.ProductInformation
{
    public static class SizeHelper
    {
        public static bool HasValue(ISizeContainer sizes)
        {                        
            return sizes.Height.HasValue ||
                   sizes.Length.HasValue || sizes.Width.HasValue ||
                   sizes.Height.HasValue || sizes.Diameter.HasValue;            
        }
        public static bool TryParse(string text, int numberElements, out int[] sizes)
        {
            int counter = 0;
            string str = string.Empty;
            sizes = new int[numberElements];
            for (int i = 0; i < text.Length; i++)
            {
                if (char.IsDigit(text[i]))
                {
                    str += text[i];
                }
                else
                {
                    if (counter > numberElements - 1)
                        break;
                    try
                    {
                        sizes[counter] = Convert.ToInt32(str);
                    }
                    catch
                    {
                        return false;
                    }
                    str = string.Empty;
                    counter++;
                }
            }
            sizes[counter] = Convert.ToInt32(str);
            return counter <= numberElements - 1;
        }
        public static SizeContainer ToSizeContainer(string format, string text)
        {
            var sizeContainer = new SizeContainer();
            int separatorsCounter = text.Where((n) => !char.IsDigit(n)).Count();
            if (TryParse(text, separatorsCounter + 1, out int[] sizes))
            {
                var formatTypes = format.Split(format.Where((n) => char.IsLower(n)).ToArray(), separatorsCounter + 1);
                for (int i = 0; i <= separatorsCounter; i++)
                {
                    switch (formatTypes[i].ToLower())
                    {
                        case "ширина":
                            sizeContainer.Width = sizes[i];
                            break;
                        case "длина":
                            sizeContainer.Length = sizes[i];
                            break;
                        case "высота":
                            sizeContainer.Height = sizes[i];
                            break;
                        case "диаметр":
                            sizeContainer.Diameter = sizes[i];
                            break;
                        default:
                            sizeContainer.AdditionalSize = sizes[i];
                            break;
                    }
                }
            }
            else throw new ArgumentException("Не удалось преобразовать размеры");
            return sizeContainer;
        }
        public static string InsertStringSizes(ISizeContainer sizes, char separator)
        {
            return (TryGetSize(sizes.Height, separator)
                + TryGetSize(sizes.Length, separator)
                + TryGetSize(sizes.Width, separator)
                + TryGetSize(sizes.Diameter, separator)
                + TryGetSize(sizes.AdditionalSize, separator)).Remove(0, 1);
        }
        private static string TryGetSize(int? size, char? separator) => size.HasValue ? separator + size.ToString() : string.Empty;
    }
}
