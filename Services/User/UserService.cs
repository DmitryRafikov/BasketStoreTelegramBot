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

        private DataContext _dataContext;
        public UserService()
        {
            _dataContext = new DataContext();
        }
        public async Task CreateUserAsync(Update update)
        {
            var recordsCount = _dataContext.Users.Count();
            var newUser = new UserEntity
            {
                Username = update.Message.Chat.Username,
                ChatID = update.Message.Chat.Id.ToString(),
                CurrentState = (int)StateTypes.Init
            };
            if (await Find(newUser))
                _dataContext.Users.Update(newUser);
            else
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
        public UserEntity GetValue(Update update)
        {
            var chatId = update.Type switch
            {
                UpdateType.CallbackQuery => update.CallbackQuery.Message.Chat.Id,
                UpdateType.Message => update.Message.Chat.Id
            };
            var user =  _dataContext.Users.FirstOrDefault(x => x.ChatID == chatId.ToString());
            if (user != null) return user;
            throw new ArgumentNullException(nameof(user));
        }
        public bool TryGetValue(string chatId, out IState state)
        {
            var user = _dataContext.Users.FirstOrDefault(x => x.ChatID == chatId);
            if (user != null)
            {
                state = user.State;
                return true;
            }
            state = null;
            return false;
        }
        public UserEntity GetValueByChatId(string chatId)
        {
            var user = _dataContext.Users.FirstOrDefault(x => x.ChatID == chatId);
            if (user != null) return user;
            throw new ArgumentNullException(nameof(user));
        }
        public async Task<bool> Find(UserEntity userEntity) {
            var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.ChatID == userEntity.ChatID ||
                                                                x.Username == userEntity.Username);
            return user != null;
        }
        public bool UserExist(Update update)
        {
            var chatId = update.Type switch
            {
                UpdateType.CallbackQuery => update.CallbackQuery.Message.Chat.Id,
                UpdateType.Message => update.Message.Chat.Id
            };
            var user = _dataContext.Users.FirstOrDefault(x => x.ChatID == chatId.ToString());
            return user != null;
        }
        public void Update(UserEntity userEntity)
        {
            _dataContext.Users.Update(userEntity);
            _dataContext.SaveChanges();
        }
    }
}
