using BasketStoreTelegramBot.Comands;
using BasketStoreTelegramBot.Features.ProductInformation;
using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.Others;
using BasketStoreTelegramBot.StateMachines;
using BasketStoreTelegramBot.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Features.States.Constructor.Sizes
{
    internal class ExistingSizeSelector:IState
    {
        public StateTypes StateType => StateTypes.OwnSizeCreator;
        private IStateMachine _stateMachine;
        private IState _returnState;
        private string? _format;
        private ProductData _currentProductData;
        private SizeContainer _sizes;
        private ShoppingBag _shoppingBag;
        public ExistingSizeSelector(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _shoppingBag = new ShoppingBag();
        }
        public ExistingSizeSelector(IStateMachine stateMachine, IState returnState)
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
            _format = _currentProductData.Format;
            var keyboard = new Markup()
            {
                KeyboardWithText = InsertSizeValues(_currentProductData.Sizes)
            };
            string text = _currentProductData.HasConstantSizes != null ? "" : " или напишите свой собственный.";
            return new TextMessage()
            {
                Text = "Выберите необходимый вам размер" + text +

                "\nВсе размеры указаны в сантиметрах в формате " + _format,
                ReplyMarkup = keyboard.Insert()
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
            try
            {
                SizeContainer enteredSizeContainer = SizeHelper.ToSizeContainer(_format, text);
                SizeContainer foundedSizeContainer = _currentProductData.Sizes.First(n => n.Equals(enteredSizeContainer));
                int cost = 0;
                if (foundedSizeContainer != null)
                {
                    _sizes = foundedSizeContainer;
                    if (foundedSizeContainer.Cost != null)
                        cost = foundedSizeContainer.Cost.Value;
                }
                SerializeProductSizes(_sizes, _shoppingBag.CurrentProduct(chatID));
                _shoppingBag.CurrentProduct(chatID).Cost = cost;
            }
            catch
            {
                var keyboard = new Markup()
                {
                    KeyboardWithText = InsertSizeValues(_currentProductData.Sizes)
                };
                return new TextMessage()
                {
                    Text = "Пожалуйста, выберите размер из предложенных",
                    ReplyMarkup = keyboard.Insert()
                };
            }
            IState state;
            bool hasSpecifics;
            if (_shoppingBag.CurrentProduct(chatID).Diameter.HasValue)
                hasSpecifics = _currentProductData.GetSpecifics(_shoppingBag.CurrentProduct(chatID).Diameter.Value).Count == 0;
            else
                hasSpecifics = false;

            if (_currentProductData.Specifics == null && !hasSpecifics)
                state = new ConstructorEnd(_stateMachine);
            else
                state = new SpecificsSelection(_stateMachine);
            if (_returnState != null)
                state = _returnState;
            _stateMachine.SetState(data.Id, state);
            return await state.Initialize(data);
        }
        private List<string> InsertSizeValues(List<SizeContainer> sizes)
        {
            List<string> allSizes = new();
            foreach (var size in sizes)
                allSizes.Add(SizeHelper.InsertStringSizes(size, 'x'));
            return allSizes;
        }
        private void SerializeProductSizes(ISizeContainer sizes, ISizeContainer product)
        {
            product.Height = sizes.Height;
            product.Width = sizes.Width;
            product.Diameter = sizes.Diameter;
            product.Length = sizes.Length;
            product.AdditionalSize = sizes.AdditionalSize;
        }
    }
}
