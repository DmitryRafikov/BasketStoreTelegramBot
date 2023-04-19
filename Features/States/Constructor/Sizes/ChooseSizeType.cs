using BasketStoreTelegramBot.Comands;
using BasketStoreTelegramBot.Entities;
using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.Features.ProductInformation;
using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.StateMachines;
using BasketStoreTelegramBot.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasketStoreTelegramBot.States;

#nullable enable

namespace BasketStoreTelegramBot.Features.States.Constructor.Sizes
{
    class ChooseSizeType : IState
    {
        public StateTypes StateType => StateTypes.ChooseSizeType;
        private ShoppingBag _shoppingBag;
        private ProductData _currentProductData;
        private readonly IStateMachine _stateMachine;
        private readonly IState _returnState;
        private bool _actionSelected = false;
        private List<string> _sizeTypeActions = new List<string>() {
            "Выбрать готовое",
            "Создать свое"
        };
        
        public ChooseSizeType(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _shoppingBag = new ShoppingBag();
        }
        public ChooseSizeType(IStateMachine stateMachine, IState returnState)
        {
            _stateMachine = stateMachine;
            _returnState = returnState;
            _shoppingBag = new ShoppingBag();
        }
        public async Task<IMessage> Initialize(MessageEvent data)
        {
            int chatID = Convert.ToInt32(data.Id);
            _currentProductData = ProductDataJsonDeserializer.Instance.
                                                   CurrentProductInfo(_shoppingBag.CurrentProduct(chatID).Name,
                                                                        _shoppingBag.CurrentProduct(chatID).BottomType);
            if (_currentProductData.Type != null)
                return DefineUserSizeType();
            return await Update(data);

        }
        public async Task<IMessage> Update(MessageEvent data)
        {
            var text = data.Message.ToLower();
            IState state = null;
            if (CommandsList.AllCommands.Contains(text))
            {
                CommandExecutor action = new CommandExecutor(_stateMachine);
                return await action.DefineCommand(text, data);
            }
            if (_currentProductData.Type != null)
            {
                if (_sizeTypeActions.Contains(data.Message) && !_actionSelected)
                {
                    _actionSelected = true;
                    switch (text)
                    {
                        case "выбрать готовое":
                            state = new ExistingSizeSelector(_stateMachine, _returnState);
                            break;
                        case "создать свое":
                            state = new OwnSizeCreator(_stateMachine, _returnState);
                            break;
                    }
                }
                else
                    return DefineUserSizeType();
            }
            else
            {
                state = new ExistingSizeSelector(_stateMachine, _returnState);
            }
            _stateMachine.SetState(data.Id, state);
            return await state.Initialize(data);
        }
        private IMessage DefineUserSizeType()
        {
            var keyboard = new Markup()
            {
                KeyboardWithText = _sizeTypeActions
            };

            return new TextMessage
            {
                Text = "Какой размер товара использовать?",
                ReplyMarkup = keyboard.Insert()
            };
        }
    }
}
