using BasketStoreTelegramBot.Comands;
using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.Features.ProductInformation;
using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.States.Constructor
{
    class BottomTypeSelector : IState
    {
        private readonly IStateMachine _stateMachine;
        private List<string> _types;
        private Markup _keyboard;
        public BottomTypeSelector(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public StateTypes StateType => StateTypes.BottomTypeSelector;

        public async Task<IMessage> Initialize(MessageEvent data)
        {
            _types = ProductDataJsonDeserializer.Instance.GetTypes(data.Message);
            _keyboard = new Markup()
            {
                KeyboardWithText = _types
            };
            return new TextMessage()
            {
                Text = "Выберите необходимый вам тип дна",
                ReplyMarkup = _keyboard.Insert()
            };
        }

        public async Task<IMessage> Update(MessageEvent data)
        {
            if (CommandsList.AllCommands.Contains(data.Message.ToLower()))
            {
                CommandExecutor action = new CommandExecutor(_stateMachine);
                return await action.DefineCommand(data.Message.ToLower(), data);
            }
            if (_types.Contains(data.Message))
            {
                var shoppingBag = new ShoppingBag();
                shoppingBag.CurrentProduct(Convert.ToInt32(data.Id)).BottomType = data.Message;
                var state = new ColorSelector(_stateMachine);
                _stateMachine.SetState(data.Id, state);
                return await state.Initialize(data);
            }
            return new TextMessage()
            {
                Text = "Ошибка вввода! Выберите тип из предложенных",
                ReplyMarkup = _keyboard.Insert()
            };
        }
    }
}
