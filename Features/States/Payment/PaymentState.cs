using BasketStoreTelegramBot.Comands;
using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.StateMachines;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.States.Payment
{
    class PaymentState : IState
    {
        IStateMachine _stateMachine;
        private readonly List<string> _buttons = new List<string>()
        {
            "Изменить",
            "Подтвердить и оплатить"
        };

        public StateTypes StateType => StateTypes.PaymentState;

        public PaymentState(IStateMachine stateMachine) {
            _stateMachine = stateMachine;
        }
        public async Task<IMessage> Initialize(MessageEvent data)
        {
            return new TextMessage
            {
                Text = "Проверьте адрес отправления. Если все верно, нажмите на кнопку подтверждения заказа или измените данные.\n\n"
            };
        }

        public async Task<IMessage> Update(MessageEvent data)
        {
            var message = data.Message.ToLower();
            if (CommandsList.AllCommands.Contains(data.Message.ToLower()))
            {
                CommandExecutor action = new CommandExecutor(_stateMachine);
                return await action.DefineCommand(data.Message.ToLower(), data);
            }
            IState state;
            if (message == "изменить")
            {
                state = new RecieverInfo(_stateMachine);
            }
            if (message == "подтвердить и оплатить")
            {
                ShoppingBag bag = new ShoppingBag();
                await bag.BuildOrderAsync(Convert.ToInt32(data));
            }
            return new TextMessage
            {
                Text = "Команда не опознана"
            };
        }
    }
}
