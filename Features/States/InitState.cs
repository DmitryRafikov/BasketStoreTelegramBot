using System;
using BasketStoreTelegramBot.StateMachines;
using System.Threading.Tasks;
using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.Comands;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections.Generic;
using BasketStoreTelegramBot.Features;

namespace BasketStoreTelegramBot.States
{
    class InitState : IState
    {
        private readonly IStateMachine _stateMachine;

        public InitState(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public StateTypes StateType => StateTypes.Init;

        public async Task<IMessage> Initialize(MessageEvent data)
        {
            var text = data.Message.ToLower();
            if (text == CommandsList.StartCommand)
            {
                var keyboard = new Markup()
                {
                    KeyboardWithText = new List<string>() {
                        "Создать корзину",
                    }
                };
                return new TextMessage
                {
                    Text = "Для начала работы с ботом нажмите на кнопку " +
                    "Создать коризну или введите команду из меню",
                    ReplyMarkup = keyboard.Insert()
                };
            }
            return new TextMessage
            {
                Text = "Команда не опознана"
            };
        }

        public async Task<IMessage> Update(MessageEvent data)
        {
            var text = data.Message.ToLower();
            if (CommandsList.AllCommands.Contains(text))
            {
                CommandExecutor action = new CommandExecutor(_stateMachine);
                return await action.DefineCommand(text, data);
            }
            if (text == "создать корзину")
            {
                var state = new ProductTypeSelector(_stateMachine);
                _stateMachine.SetState(data.Id, state);
                return await state.Initialize(data);
            }
            var keyboard = new Markup()
            {
                KeyboardWithText = new List<string>() {
                        "Создать корзину",
                    }
            };
            if (text == CommandsList.StartCommand)
            {                
                return new TextMessage
                {
                    Text = "Для начала работы с ботом нажмите на кнопку " +
                    "Создать коризну или введите команду из меню",
                    ReplyMarkup = keyboard.Insert()
                };
            }
            return new TextMessage
            {
                Text = "Команда не опознана!",
                ReplyMarkup = keyboard.Insert()
            };
        }
    }
}
