using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BasketStoreTelegramBot
{
    class PhotoMessage:IMessage
    {
        public string Link = null;
        public string Caption = null;
        public IReplyMarkup ReplyMarkup;
        public async Task SendAsync(Chat chat, TelegramBotClient bot)
        {
            if(ReplyMarkup == null) ReplyMarkup = new ReplyKeyboardRemove();
            if (string.IsNullOrEmpty(Link))
                throw new ArgumentException("Отсутствует ссылка на файл");
            await bot.SendPhotoAsync(chat.Id, Link, Caption, replyMarkup: ReplyMarkup);
        }
    }
}
