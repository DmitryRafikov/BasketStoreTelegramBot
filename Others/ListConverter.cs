using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Others
{
    class ListConverter
    {
        public static string ToString(List<string> str)
        {
            string data = string.Empty;
            for (int i = 0; i < str.Count; i++)
            {
                data += str[i];
                if (str[i] != str.Last())
                    data += ", ";
            }
            return data;
        }
        public static string ToString(List<string> str, string separator)
        {
            string data = string.Empty;
            for (int i = 0; i < str.Count; i++)
            {
                data += str[i];
                if (str[i] != str.Last())
                    data += separator;
            }
            return data;
        }
    }
}
