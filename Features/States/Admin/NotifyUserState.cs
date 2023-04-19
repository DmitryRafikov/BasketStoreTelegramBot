using BasketStoreTelegramBot.Comands;
using BasketStoreTelegramBot.Entities;
using BasketStoreTelegramBot.Features.Messages;
using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.Services.Order;
using BasketStoreTelegramBot.StateMachines;
using BasketStoreTelegramBot.States;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Features.States.Admin
{
    class UserNotificationState : IState
    {
        public StateTypes StateType => StateTypes.UserNotificationState;
        private IStateMachine _stateMachine;
        private IOrderService _orderService;
        private UserNotifyer _userNotifyer;
        private List<string> _buttons;
        public UserNotificationState(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _userNotifyer = new UserNotifyer();
            _orderService = new OrderService();
            _buttons = new List<string>() { 
            "Вернуться назад"
            };
        }

        public async Task<IMessage> Initialize(MessageEvent data)
        {
            return new TextMessage
            {
                Text = "Введите номер заказа"
            };
        }

        public async Task<IMessage> Update(MessageEvent data)
        {
            if (CommandsList.AllCommands.Contains(data.Message.ToLower()))
            {
                CommandExecutor action = new CommandExecutor(_stateMachine);
                return await action.DefineCommand(data.Message.ToLower(), data);
            }
            Markup keyboard = new Markup() {
                KeyboardWithText = _buttons
            };
            OrderEntity order = _orderService.GetOrderById(Convert.ToInt32(data.Message));
            INotifyMessage message = new TextMessage
            {
                Text = $"Мы приняли ваш заказ {order.Id} и начали его вязать"
            };
            if (order != null)
            {
                await _userNotifyer.Notify(message, order.ChatID);
                return new TextMessage
                {
                    Text = "Пользователь уведомлен!",
                    ReplyMarkup = keyboard.Insert()
                };
            }
            if (data.Message.ToLower() == "вернуться назад")
            {
                var state = new AdminControlsState(_stateMachine);
                _stateMachine.SetState(data.Id, state);
                return await state.Initialize(data);
            }
            return new TextMessage
            {
                Text = "Заказ с таким номером не найден",
                ReplyMarkup = keyboard.Insert()
            };

        }
    }
}
