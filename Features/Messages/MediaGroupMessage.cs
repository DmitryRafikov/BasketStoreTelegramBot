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
        public MediaGroup Data { get; set; }
        public string? Caption { get; set; }
        public ReplyKeyboardMarkup? ReplyMarkup { get; set; }
        public string? TextMessageToChangeKeyboard { get; set; }
        public async Task SendAsync(Chat chat, TelegramBotClient bot)
        {
            if (ReplyMarkup != null && (TextMessageToChangeKeyboard != null || TextMessageToChangeKeyboard != string.Empty))
            {
               await bot.SendTextMessageAsync(chat.Id, TextMessageToChangeKeyboard, replyMarkup: ReplyMarkup);
            }
            var message = await bot.SendMediaGroupAsync(chat.Id, Data.Insert());
            if (Caption != null || Caption != string.Empty)                
                await bot.EditMessageCaptionAsync(chat.Id, message[message.Length - 1].MessageId, Caption);
        }
    }
}
