using BasketStoreTelegramBot.Features;

namespace BasketStoreTelegramBot.Services.Admin
{
    interface IAdminService
    {
        public bool AdminExists(MessageEvent data);
    }
}
