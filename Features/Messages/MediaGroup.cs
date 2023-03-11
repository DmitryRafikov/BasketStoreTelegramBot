using System.Collections.Generic;
using Telegram.Bot.Types;
namespace BasketStoreTelegramBot.MessagesHandle
{
    class MediaGroup
    {
        public List<string> Data;
        public MediaGroup(string[] data) {
            Data = new List<string>();
            if (data.Length != 0)
                foreach (var item in data) {
                    Data.Add(item);
                }
        }
        public MediaGroup() {
            Data = new List<string>();
        }
        public IAlbumInputMedia[] Insert() 
        {
            List<InputMediaPhoto> data = new List<InputMediaPhoto>();
            foreach (var item in Data)
                data.Add(new InputMediaPhoto(item));
            return data.ToArray();
        }
    }
}
