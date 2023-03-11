using BasketStoreTelegramBot.Entities;
using BasketStoreTelegramBot.Models;
using BasketStoreTelegramBot.StateMachines;
using BasketStoreTelegramBot.States;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BasketStoreTelegramBot.Services.User
{
    class UserService : IUserService
    {
        private static Lazy<UserService> _userService = new Lazy<UserService>(() => new UserService());
        public static UserService Instance { get => _userService.Value; }

        private DataContext _dataContext = DataContext.Instance;
        public UserService()
        {

        }
        public async Task CreateUserAsync(Update update)
        {
            var recordsCount = _dataContext.Users.Count();
            var newUser = new UserEntity
            {
                ID = recordsCount++,
                Username = update.Message.Chat.Username,
                ChatID = update.Message.Chat.Id.ToString(),
                CurrentState = (int)StateTypes.Init
            };
            await _dataContext.Users.AddAsync(newUser);
            await _dataContext.SaveChangesAsync();
        }
        public async Task<UserEntity> GetValueAsync(Update update)
        {
            var chatId = update.Type switch
            {
                UpdateType.CallbackQuery => update.CallbackQuery.Message.Chat.Id,
                UpdateType.Message => update.Message.Chat.Id
            };
            var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.ChatID == chatId.ToString());
            if (user != null) return user;
            throw new ArgumentNullException(nameof(user));
        }
    }
}
