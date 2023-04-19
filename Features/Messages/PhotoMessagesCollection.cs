using BasketStoreTelegramBot.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BasketStoreTelegramBot.MessagesHandle
{
    class PhotoMessagesCollection : IMessage
    {
        public List<PhotoMessage> Collection;
        
        public async Task SendAsync(Chat chat, TelegramBotClient bot)
        {
            foreach (var item in Collection)
            {
                await item.SendAsync(chat, bot);
            }
            await ChangeMarkupType(chat, bot);
        }

        private async Task ChangeMarkupType(Chat chat, TelegramBotClient bot) {
            var _button = new List<string>() { "Оформить заказ" };
            var keyboard = new Markup() {
                KeyboardWithText = _button
            };
            var message = new TextMessage()
            {
                Text = "Показаны все товары",
                ReplyMarkup = keyboard.Insert()
            };
            await message.SendAsync(chat, bot);
        }
    }
}
