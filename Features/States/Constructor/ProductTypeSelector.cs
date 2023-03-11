using BasketStoreTelegramBot.Comands;
using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.Features.ProductInformation;
using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.StateMachines;
using BasketStoreTelegramBot.States.Constructor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.States
{
    class ProductTypeSelector : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly List<string> _products = ProductDataJsonDeserializer.Instance.GetNames();
        private List<string> _types = new List<string>();
        public ProductTypeSelector(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public StateTypes StateType => StateTypes.ProductTypeSelector;

        public async Task<IMessage> Initialize(MessageEvent data)
        {

           ShoppingBag.Instance.CreateProduct();
            var keyboard = new Markup()
            {
                KeyboardWithText = _products
            };
            return new PhotoMessage()
            {
                Caption = "Давайте начнем! Выберите тип товара из предложенных:",
                Link = "https://sun9-north.userapi.com/sun9-84/s/v1/ig2/eNtfKbTn2LIr3UssDXK2x1jGNkobmoaFkg186BLuAa7kNXR1AfT40oMcI9pAy9V-lPMrK0ZL9kimbzLKZj5y9BK2.jpg?size=533x943&quality=96&type=album",
                ReplyMarkup = keyboard.Insert()
            };
        }

        public async Task<IMessage> Update(MessageEvent data)
        {
            var text = data.Message.ToLower();
            var productData = ProductDataJsonDeserializer.Instance;
            var keyboard = new Markup()
            {
                KeyboardWithText = ProductDataJsonDeserializer.Instance.GetNames()
            };
            if (CommandsList.AllCommands.Contains(text))
            {
                CommandExecutor action = new CommandExecutor(_stateMachine);
                return await action.DefineCommand(text, data);
            }
            if (_products.Contains(data.Message)) 
            {
                ShoppingBag.Instance.CurrentProduct.Name = data.Message;
                IState state;
                if (productData.CurrentProductInfo(data.Message).Type != null)
                    state = new BottomTypeSelector(_stateMachine);
                else
                    state = new ColorSelector(_stateMachine);
                _stateMachine.SetState(data.Id, state);
                return await state.Initialize(data);
            }            
            return new TextMessage()
            {
                Text = "Команда не опознана! Выберите тип товара",
                ReplyMarkup = keyboard.Insert()
            };
        }
    }
}
