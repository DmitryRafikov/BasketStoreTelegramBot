using BasketStoreTelegramBot.Features.Messages;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BasketStoreTelegramBot.MessagesHandle
{
    class TextMessage : IMessage, INotifyMessage
    {
        public string Text { get; set; }
        public IReplyMarkup ReplyMarkup = new ReplyKeyboardRemove();
        public async Task SendAsync(Chat chat, TelegramBotClient bot)
        {
            await bot.SendTextMessageAsync(chat.Id, Text, replyMarkup: ReplyMarkup);
        }
        public async Task NotifyAsync(ChatId id, TelegramBotClient bot)
        {
            await bot.SendTextMessageAsync(id, Text, replyMarkup: ReplyMarkup);
        }
    }
}
