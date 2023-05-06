using BasketStoreTelegramBot.Comands;
using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.Features.ProductInformation;
using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.StateMachines;
using BasketStoreTelegramBot.States.Constructor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.States
{
    class ProductTypeSelector : IState
    {
        public StateTypes StateType => StateTypes.ProductTypeSelector;
        private readonly IStateMachine _stateMachine;
        private ShoppingBag _shoppingBag;
        private readonly List<string> _products = ProductDataJsonDeserializer.Instance.GetNames();
        private List<string> _imagesURLs = new List<string>() {
            "https://sun9-58.userapi.com/impg/ExbZ7_1nc9W0wgJQF_2aniPsHuSZo5lKPEbvYg/vhskMkwQO0o.jpg?size=960x1280&quality=95&sign=63c6447f048f8485b34bd52f0cc5c353&type=album",
            "https://sun9-27.userapi.com/impg/aW0fECkZ10ssNx3JKJ45cb5GBWDX2cPmUgCIkw/XWrs8YyTN9g.jpg?size=960x1280&quality=95&sign=05b0f3edabb59d467d6b559626443b25&type=album",
            "https://sun9-61.userapi.com/impg/dI06AJqyADe0kNRYdN56QJsn24Ddx38KJ9fC8A/YxeWOrivSFA.jpg?size=960x1280&quality=95&sign=e365b6b4276411cca78eead43e750e3b&type=album",
            "https://sun9-59.userapi.com/impg/1H7CKie8yadQd4SyO9gMwfDKbaklvP6SgCPYOw/gP7mvb0CWz8.jpg?size=960x1280&quality=95&sign=e0fd1d97de7d485762657e3da15530c1&type=album",
            "https://sun9-8.userapi.com/impg/kN7Zo08ATx30MaZNLABi7TdTtxPJynRDip6i4A/axX1kxqdOvs.jpg?size=960x1280&quality=95&sign=744561f2f76d8618b8f564fa92ec3b4d&type=album"
        };
        public ProductTypeSelector(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _shoppingBag = new ShoppingBag();
        }


        public async Task<IMessage> Initialize(MessageEvent data)
        {
            _shoppingBag.CreateProduct(Convert.ToInt32(data.Id));
            var keyboard = new Markup()
            {
                KeyboardWithText = _products
            };
            return new MediaGroupMessage() {
                ReplyMarkup = keyboard.Insert(),
                TextMessageToChangeKeyboard = "Давайте начнем!",
                Caption =  "Выберите тип товара из предложенных:",
                Data = new MediaGroup()
                {
                    ItemsURLs = _imagesURLs
                }
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
                _shoppingBag.CurrentProduct(Convert.ToInt32(data.Id)).Name = data.Message;
                var product = _shoppingBag.CurrentProduct(Convert.ToInt32(data.Id));
                await _shoppingBag.UpdateInfo(product);
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
