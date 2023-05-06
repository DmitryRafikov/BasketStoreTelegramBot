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

        public StateTypes StateType => StateTypes.ColorSelector;

        public ColorSelector(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _shoppingBag = new ShoppingBag();
        }
        public ColorSelector(IStateMachine stateMachine, IState returnState)
        {
            _stateMachine = stateMachine;
            _returnState = returnState;
            _shoppingBag = new ShoppingBag();
        }


        public async Task<IMessage> Initialize(MessageEvent data)
        {
            int chatID = Convert.ToInt32(data.Id);
            var product = GetProductData(chatID);
            return new PhotoMessage()
            {
                Caption = "Выберите необходимый цвет",
                Link = product.PalettePhoto,
            };
        }

        public async Task<IMessage> Update(MessageEvent data)
        {
            int chatID = Convert.ToInt32(data.Id);
            var productData = GetProductData(chatID);
            var colors = productData.Palette;
            var text = data.Message.ToLower();
            if (CommandsList.AllCommands.Contains(text))
            {
                CommandExecutor action = new CommandExecutor(_stateMachine);
                return await action.DefineCommand(text, data);
            }
            if (colors.Contains(text))
            {
                var product = _shoppingBag.CurrentProduct(chatID);
                product.Color = text;
                await _shoppingBag.UpdateInfo(product);
                IState state;
                if (GetProductData(chatID).Sizes.Count != 0)
                    state = new ChooseSizeType(_stateMachine);
                else if (productData.Specifics.Count != 0)
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
        private ProductData GetProductData(int chatID)
        {
            var product = _shoppingBag.CurrentProduct(chatID);
            var name = product.Name;
            var bottom = product.BottomType;
            if (bottom != null)
                return ProductDataJsonDeserializer.Instance.CurrentProductInfo(name, bottom);
            return ProductDataJsonDeserializer.Instance.CurrentProductInfo(name);
        }
    }
}
