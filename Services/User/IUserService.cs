using BasketStoreTelegramBot.Entities;
using BasketStoreTelegramBot.StateMachines;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace BasketStoreTelegramBot.Services.User
{
    interface IUserService
    {
        public bool UserExist(Update update);
        public Task<UserEntity> GetValueAsync(Update update);
        UserEntity GetValue(Update update);
        public Task CreateUserAsync(Update update);
    }
}
