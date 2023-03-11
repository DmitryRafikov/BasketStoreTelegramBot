using BasketStoreTelegramBot.Features.Messages;
using Telegram.Bot.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Features
{
    class UserNotifyer
    {
        private MessagesFactory _factory = MessagesFactory.Instance;

        public async Task Notify(INotifyMessage message, string userId)
        {
            await _factory.Notify(Convert.ToInt64(userId), message);
        }
    }
}
