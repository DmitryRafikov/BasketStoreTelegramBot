using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.Models;
using System;
using System.Linq;

namespace BasketStoreTelegramBot.Services.Admin
{
    class AdminService : IAdminService
    {
        private DataContext _dataContext;
        public AdminService()
        {
            _dataContext = new DataContext();
        }
        public bool AdminExists(MessageEvent data)
        {
            var admin = _dataContext.Admins.FirstOrDefault(x => x.DialogueID == data.Id);
            if (admin != null)
                return true;
            return false;
        }
    }
}
