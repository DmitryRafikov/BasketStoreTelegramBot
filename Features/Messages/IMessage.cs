using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BasketStoreTelegramBot
{
    public interface IMessage
    {
        public Task SendAsync(Chat chat, TelegramBotClient bot);
    }
}
