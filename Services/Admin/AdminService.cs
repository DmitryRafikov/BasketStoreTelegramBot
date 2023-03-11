using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.Models;
using System;
using System.Linq;

namespace BasketStoreTelegramBot.Services.Admin
{
    class AdminService : IAdminService
    {
        private Lazy<AdminService> _adminService = new Lazy<AdminService>(() => new AdminService());
        private AdminService Instance { get => _adminService.Value; }
        private DataContext _dataContext = DataContext.Instance;
        public bool AdminExists(MessageEvent data)
        {
            var admin = _dataContext.Admins.FirstOrDefault(x => x.DialogueID == data.Id);
            if (admin != null)
                return true;
            return false;
        }
    }
}
