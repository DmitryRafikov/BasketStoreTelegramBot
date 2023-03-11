using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.StateMachines;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.Features.ProductInformation;

namespace BasketStoreTelegramBot.States
{
    class ConstructorEnd : IState
    {
        private readonly IStateMachine _stateMachine;
        private static ProductData _currentProductInfo = ProductDataJsonDeserializer.Instance.
                                           CurrentProductInfo(ShoppingBag.Instance.CurrentProduct.Name,
                                                                ShoppingBag.Instance.CurrentProduct.BottomType);
        private readonly List<string> _buttons = new List<string>() {
            "Изменить",
            "Добавить в отложенные",
            "Вернуться в начало"
        };

        public StateTypes StateType => StateTypes.ConstructorEnd;

        public ConstructorEnd(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private List<string> _changeButtons()
        {
            List<string> buttons = new();
            var product = ShoppingBag.Instance.CurrentProduct;
            buttons.Add("Изменить цвет");
            if (_currentProductInfo.Resizable.Value)
            {
                buttons.Add("Изменить размер");
            }
            if (_currentProductInfo.Specifics != null)
            {
                buttons.Add("Изменить особенности");
            }
            return buttons;
        }
        public async Task<IMessage> Initialize(MessageEvent data)
        {

            var keyboard = new Markup()
            {
                KeyboardWithText = _buttons
            };
            var product = ShoppingBag.Instance.CurrentProduct;
            return new PhotoMessage()
            {
                Link = _currentProductInfo.Photo,
                ReplyMarkup = keyboard.Insert(),
                Caption = product.ToString(),
            };
            
        }

        public async Task<IMessage> Update(MessageEvent data)
        {
            var text = data.Message.ToLower();
            if (_buttons.Contains(data.Message))
            {
                if (text == "добавить в отложенные")
                {
                    await ShoppingBag.Instance.EndProductSerializationAsync();
                    return new TextMessage
                    {
                        Text = "Добавлено в отложенные"
                    };
                }
                IState state = null;
                if (text == "вернуться в начало")
                {
                    state = new InitState(_stateMachine);
                }
                if (text == "изменить")
                {
                    state = new ChracteristicsChanger(_stateMachine);
                }
                _stateMachine.SetState(data.Id, state);
                return await state.Initialize(data);
            }
            return new TextMessage { 
                Text = "Команда не опознана!"
            };
        }

    }
}
