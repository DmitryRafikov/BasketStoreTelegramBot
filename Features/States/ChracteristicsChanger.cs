using BasketStoreTelegramBot.Comands;
using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.Features.ProductInformation;
using BasketStoreTelegramBot.Features.States.Constructor.Sizes;
using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.StateMachines;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.States
{
    class ChracteristicsChanger:IState
    {
        private readonly IStateMachine _stateMachine;
        private ShoppingBag _shoppingBag;
        private ProductData _currentProductInfo;
        public StateTypes StateType => StateTypes.CharacteristicsChanger;

        private List<string> DefineButtons(MessageEvent data)
        {
            int chatID = Convert.ToInt32(data.Id);
            List<string> buttons = new();
            var product = GetProductData(chatID);
            buttons.Add("Изменить цвет");
            if (product.Resizable.Value)
            {
                buttons.Add("Изменить размер");
            }
            if (product.Specifics != null)
            {
                buttons.Add("Изменить особенности");
            }
            buttons.Add("Отмена");
            return buttons;
        }

        public ChracteristicsChanger(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _shoppingBag = new ShoppingBag();
        }

        public async Task<IMessage> Initialize(MessageEvent data)
        {            
            var keyboard = new Markup
            {
                KeyboardWithText = DefineButtons(data)
            };
            return new TextMessage
            {
                Text = "Выберите, что бы вы хотели изменить",
                ReplyMarkup = keyboard.Insert()
            };
        }

        public async Task<IMessage> Update(MessageEvent data)
        {
            int chatID = Convert.ToInt32(data.Id);
            if (CommandsList.AllCommands.Contains(data.Message.ToLower()))
            {
                CommandExecutor action = new CommandExecutor(_stateMachine);
                return await action.DefineCommand(data.Message.ToLower(), data);
            }
            if (DefineButtons(data).Contains(data.Message)) {
                IState state = null;
                if (data.Message == "Изменить цвет")
                {
                    state = new ColorSelector(_stateMachine, this);
                }
                if (data.Message == "Изменить размер")
                {
                    state = new ChooseSizeType(_stateMachine, this);
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
        private ProductData GetProductData(int chatID)
        {
            var name = _shoppingBag.CurrentProduct(chatID).Name;
            var bottom = _shoppingBag.CurrentProduct(chatID).BottomType;
            if (bottom != null)
                return ProductDataJsonDeserializer.Instance.CurrentProductInfo(name, bottom);
            return ProductDataJsonDeserializer.Instance.CurrentProductInfo(name);
        }
    }
}
