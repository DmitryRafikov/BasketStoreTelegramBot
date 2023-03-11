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
        private static MessagesFactory _factory;
        static void Main(string[] args)
        {
            _stateMachine = new StateMachine(CreateInitState);
            _bot = new TelegramBotClient("5846422307:AAFe--2RJpGiSwQQOEO3KxaxkyQUdIxUMgE");
            _userService = UserService.Instance;
            StartBot();

            Thread.Sleep(Timeout.Infinite);
        }
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == UpdateType.Message || update.Type == UpdateType.CallbackQuery)
            {
                var chat = update.Message.Chat;
                var message = update.Message;
                var callback = update.CallbackQuery;

                if (chat == null || ((message == null || string.IsNullOrEmpty(message.Text)) && callback == null)) return;

                var data = new MessageEvent
                {
                    Id = chat.Id.ToString(),
                    Message = message.Text,
                    Callback = callback
                };
                try
                {
                    var user = await _userService.GetValueAsync(update);
                }
                catch
                {
                    await _userService.CreateUserAsync(update);
                }
                var result = await _stateMachine.FireEvent(data);
                await MessagesFactory.Instance.SendAsync(chat, result);
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
        static IState CreateInitState()
        {
            return new InitState(_stateMachine);
        }
    }
}
