using BasketStoreTelegramBot.Entities;
using BasketStoreTelegramBot.StateMachines;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace BasketStoreTelegramBot.Services.User
{
    interface IUserService
    {
        public Task<UserEntity> GetValueAsync(Update update);
        public Task CreateUserAsync(Update update);
    }
}
