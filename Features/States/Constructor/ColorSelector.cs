using BasketStoreTelegramBot.Comands;
using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.StateMachines;
using System.Threading.Tasks;
using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.Features.ProductInformation;
using BasketStoreTelegramBot.Features.States.Constructor.Sizes;
using System;

namespace BasketStoreTelegramBot.States
{
    class ColorSelector : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IState? _returnState;
        private ShoppingBag _shoppingBag;
        private ProductData _currentProductData;
        private string _colors;

        public StateTypes StateType => StateTypes.ColorSelector;

        public ColorSelector(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        public ColorSelector(IStateMachine stateMachine, IState returnState)
        {
            _stateMachine = stateMachine;
            _returnState = returnState;
        }


        public async Task<IMessage> Initialize(MessageEvent data)
        {
            int chatID = Convert.ToInt32(data.Id);
            _currentProductData = ProductDataJsonDeserializer.Instance.
                                                    CurrentProductInfo(_shoppingBag.CurrentProduct(chatID).Name);
            _colors = _currentProductData.Palette;
            return new PhotoMessage()
            {
                Caption = "Выберите необходимый цвет",
                Link = _currentProductData.PalettePhoto,
            };
        }

        public async Task<IMessage> Update(MessageEvent data)
        {
            int chatID = Convert.ToInt32(data.Id);
            var text = data.Message.ToLower();
            if (CommandsList.AllCommands.Contains(text))
            {
                CommandExecutor action = new CommandExecutor(_stateMachine);
                return await action.DefineCommand(text, data);
            }
            if (_colors.Contains(text))
            {
                _shoppingBag.CurrentProduct(chatID).Color = text;
                IState state;
                if (_currentProductData.Sizes.Count != 0)
                    state = new ChooseSizeType(_stateMachine);
                else if (_currentProductData.Specifics.Count != 0)
                    state = new SpecificsSelection(_stateMachine);
                else
                    state = new ConstructorEnd(_stateMachine);
                if (_returnState != null)
                    state = _returnState;

                _stateMachine.SetState(data.Id, state);
                return await state.Initialize(data);
            }
            return new TextMessage()
            {
                Text = "Команда не опознана! Повторите ввод"
            };
        }
    }
}
