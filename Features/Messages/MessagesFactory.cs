using BasketStoreTelegramBot.Features.Messages;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BasketStoreTelegramBot
{
    class MessagesFactory
    {
        private static readonly Lazy<MessagesFactory> _orderService = new Lazy<MessagesFactory>(() => new MessagesFactory());
        public static MessagesFactory Instance { get => _orderService.Value; } 
        public TelegramBotClient Bot { get => new TelegramBotClient("5846422307:AAGgssW9osAbTUgUWRVcRQPoxHKmj6dc9XA"); }
        private MessagesFactory() 
        {
        }
        public async Task SendAsync(Chat chat, IMessage message) 
        {
            await message.SendAsync(chat, Bot);
        }
        public async Task Notify(ChatId id, INotifyMessage message)
        {
            await message.NotifyAsync(id, Bot);
        }
    }
}
