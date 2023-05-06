using System.Collections.Generic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using System.Linq;
namespace BasketStoreTelegramBot.MessagesHandle
{
    class Markup
    {
        public List<string> KeyboardWithText;
        public Dictionary<string,string> KeyboardWithCallBack;
        public bool Resizable = true;
        public Markup()
        {
            KeyboardWithText = new List<string>();
            KeyboardWithCallBack = new Dictionary<string, string>();
        }
        public ReplyKeyboardMarkup Insert()
        {
            List<KeyboardButton[]> keyboard = new List<KeyboardButton[]>();
            int counter = 0;
            while (counter < KeyboardWithText.Count)
            {
                var len = counter == KeyboardWithText.Count - 1 &&
                           KeyboardWithText.Count % 2 == 1 ? 1 : 2;
                KeyboardButton[] buttons = new KeyboardButton[len];
                for (int i = 0; i < len; i++)
                {
                    buttons[i] = KeyboardWithText[counter];
                    counter++;
                }
                keyboard.Add(buttons);
            }
            return new(keyboard) { ResizeKeyboard = Resizable };
        }
        public InlineKeyboardMarkup Insert(bool hasCallBackData)
        {

            var keyboard = new List<InlineKeyboardButton[]>();                                  
            int counter = 0;
            while (counter < KeyboardWithCallBack.Count)
            {
                var len = counter == KeyboardWithCallBack.Count - 1 &&
                           KeyboardWithCallBack.Count % 2 == 1 ? 1 : 2;
                InlineKeyboardButton[] buttons = new InlineKeyboardButton[len];
                for (int i = 0; i < len; i++)
                {
                    if (hasCallBackData)
                        buttons[i] = InlineKeyboardButton.WithCallbackData(
                            text: KeyboardWithCallBack.Keys.ElementAt(counter),
                            callbackData: KeyboardWithCallBack.Values.ElementAt(counter)
                        );
                    counter++;
                }
                keyboard.Add(buttons);
            }
            return new(keyboard);
        }
    }
}
