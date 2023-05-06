using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.Services.User;
using BasketStoreTelegramBot.StateMachines;
using BasketStoreTelegramBot.States;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BasketStoreTelegramBot
{
    class Program
    {
        private static TelegramBotClient _bot;
        private static IStateMachine _stateMachine;
        private static IUserService _userService;
        static void Main(string[] args)
        {
            _bot = new TelegramBotClient("5846422307:AAGgssW9osAbTUgUWRVcRQPoxHKmj6dc9XA");
            _userService = new UserService();
            _stateMachine = new StateMachine();
            StartBot();
            Thread.Sleep(Timeout.Infinite);
        }
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == UpdateType.Message || update.Type == UpdateType.CallbackQuery)
            {
                Chat chat;
                Message? message = update.Message;
                CallbackQuery? callback = update.CallbackQuery; 
                if (update.Type == UpdateType.CallbackQuery)
                {
                    chat = callback.Message.Chat;
                }
                else 
                {
                    chat = message.Chat;
                }

                if (chat == null || ((message == null || string.IsNullOrEmpty(message.Text)) && callback == null)) return;

                var data = new MessageEvent
                {
                    Id = chat.Id.ToString(),
                    Message = message == null? string.Empty: message.Text,
                    Callback = callback
                };
                if(_userService.UserExist(update))
                    _stateMachine.GetLastActiveState(update);
                else
                    await _userService.CreateUserAsync(update);
                var result = await _stateMachine.FireEvent(data, update);
                try
                {
                    await MessagesFactory.Instance.SendAsync(chat, result);
                }
                catch { }
            }
        }
        public static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
        static void StartBot()
        {
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };
            _bot.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );
        }
        
    }
}
