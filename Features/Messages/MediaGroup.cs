using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;
namespace BasketStoreTelegramBot.MessagesHandle
{
    class MediaGroup
    {
        public List<string> ItemsURLs { get; set; }
        //public IEnumerable<IAlbumInputMedia> Media { get; set; }
        public MediaGroup(string[] data) {
            ItemsURLs = new List<string>();
            if (data.Length != 0)
                foreach (var item in data) {
                    ItemsURLs.Add(item);
                }
        }
        public MediaGroup() { }
        public IAlbumInputMedia[] Insert() 
        {
            List<InputMediaPhoto> data = new List<InputMediaPhoto>();
            foreach (var item in ItemsURLs)
            {
                data.Add(new InputMediaPhoto(item));
            }
            return data.ToArray();
        }
    }
}
