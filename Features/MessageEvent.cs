using Telegram.Bot.Types;

namespace BasketStoreTelegramBot.Features
{
    public class MessageEvent
    {
        public string Id { get; set; }
        public string? Message { get; set; }
        public CallbackQuery Callback { get; set; }
    }
}