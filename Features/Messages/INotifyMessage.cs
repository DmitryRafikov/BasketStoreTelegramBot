using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BasketStoreTelegramBot.Features.Messages
{
    interface INotifyMessage
    {
        Task NotifyAsync(ChatId id, TelegramBotClient bot);
    }
}
