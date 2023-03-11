using BasketStoreTelegramBot.MessagesHandle;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BasketStoreTelegramBot
{
    class MediaGroupMessage : IMessage
    {
        public string Caption = null;
        public IReplyMarkup ReplyMarkup = new ReplyKeyboardRemove();
        public MediaGroup Data;
        public async Task SendAsync(Chat chat, TelegramBotClient bot)
        {
            //await bot.SendMediaGroupAsync(chat.Id, Data.Insert(), Caption);
        }
    }
}
