using System;
using BasketStoreTelegramBot.StateMachines;
using System.Threading.Tasks;
using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.Comands;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections.Generic;
using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.Features.States;

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
        List<string> _buttons = new()
        {
            "Создать свое",
            "Выбрать готовое"
        };

        public async Task<IMessage> Initialize(MessageEvent data)
        {
            var text = data.Message.ToLower();
            if (text == CommandsList.StartCommand)
            {
                var keyboard = new Markup()
                {
                    KeyboardWithText = _buttons
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
            if (_buttons.Contains(data.Message))
            {
                IState state = null;
                if (text == "создать свое")
                {
                    state = new ProductTypeSelector(_stateMachine);

                }
                if (text == "выбрать готовое")
                {
                    state = new CatalogState(_stateMachine);
                }

                _stateMachine.SetState(data.Id, state);
                return await state.Initialize(data);
            }
            var keyboard = new Markup()
            {
                KeyboardWithText = _buttons
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
