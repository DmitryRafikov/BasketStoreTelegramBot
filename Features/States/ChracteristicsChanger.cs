using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.Features.ProductInformation;
using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.StateMachines;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.States
{
    class ChracteristicsChanger:IState
    {
        private readonly IStateMachine _stateMachine;
        private static ProductData _currentProductInfo = ProductDataJsonDeserializer.Instance.
                                   CurrentProductInfo(ShoppingBag.Instance.CurrentProduct.Name,
                                                        ShoppingBag.Instance.CurrentProduct.BottomType);

        private readonly List<string> _buttons = DefineButtons();

        public StateTypes StateType => StateTypes.CharacteristicsChanger;

        private static List<string> DefineButtons()
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
            buttons.Add("Отмена");
            return buttons;
        }

        public ChracteristicsChanger(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public async Task<IMessage> Initialize(MessageEvent data)
        {
            var keyboard = new Markup
            {
                KeyboardWithText = _buttons
            };
            return new TextMessage
            {
                Text = "Выберите, что бы вы хотели изменить",
                ReplyMarkup = keyboard.Insert()
            };
        }

        public async Task<IMessage> Update(MessageEvent data)
        {
            var text = data.Message.ToLower();
            if (_buttons.Contains(data.Message)) {
                IState state = null;
                if (data.Message == "Изменить цвет")
                {
                    state = new ColorSelector(_stateMachine, this);
                }
                if (data.Message == "Изменить размер")
                {
                    state = new SizeSelector(_stateMachine, this);
                }
                if (data.Message == "Изменить особенности")
                {
                    state = new SpecificsSelection(_stateMachine, this);
                }
                if (data.Message == "Отмена")
                {
                    state = new ConstructorEnd(_stateMachine);
                }
                _stateMachine.SetState(data.Id, state);
                return await state.Initialize(data);
            }
            return new TextMessage
            {
                Text = "Команда не опознана!"
            };
        }
    }
}
