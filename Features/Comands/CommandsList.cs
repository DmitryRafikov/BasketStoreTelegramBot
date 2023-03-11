using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Comands
{
    public class CommandsList 
    {
        public const string StartCommand = "/start";
        public const string ConstructorCommand = "/constructor";
        public const string CatalogCommand = "/catalog";
        public const string ShowDelayed = "/delayed";
        public const string ShowDeliveryInfo = "/deliveryinfo";
        public const string ChangeToAdmin = "/admin";
        public static List<string> AllCommands = new List<string>()
        {
            StartCommand,
            ConstructorCommand,
            CatalogCommand,
            ShowDelayed,
            ShowDeliveryInfo,
            ChangeToAdmin
        };
    }
}
