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
    internal class OwnSizeCreator : IState
    {
        public StateTypes StateType => StateTypes.OwnSizeCreator;
        public IStateMachine _stateMachine;
        private IState _returnState;
        private SizeContainer _sizes;
        private ShoppingBag _shoppingBag;
        private string? format;
        private ProductData _currentProductData;
        public OwnSizeCreator(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        public OwnSizeCreator(IStateMachine stateMachine, IState returnState)
        {
            _stateMachine = stateMachine;
            _returnState = returnState;
        }

        public async Task<IMessage> Initialize(MessageEvent data)
        {
            int chatID = Convert.ToInt32(data.Id);
            _currentProductData = ProductDataJsonDeserializer.Instance.
                                    CurrentProductInfo(_shoppingBag.CurrentProduct(chatID).Name,
                                                        _shoppingBag.CurrentProduct(chatID).BottomType);
            format = _currentProductData.Format.Replace("ВЫСОТАх", "");
            var keyboard = new Markup()
            {
                KeyboardWithText = InsertSizeValues(_currentProductData.Sizes)
            };
            return new TextMessage()
            {
                Text = "Выберите необходимый вам размер" +

                "\nРазмеры указаны в сантиметрах в формате " + format,
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
                if (_sizes == null)
                {
                    _sizes = SizeHelper.ToSizeContainer(format, text);
                    return new TextMessage()
                    {
                        Text = "Напишите необходимую вам высоту корзины",
                    };
                }
                try {
                    _sizes.Height = Convert.ToInt32(text);
                }
                catch {
                    return new TextMessage()
                    {
                        Text = "Ошибка преобразования размера. Повторите ввод",
                    };
                }
                ISizeContainer enteredSizeContainer = SizeHelper.HasValue(_sizes) ? _sizes : SizeHelper.ToSizeContainer(format, text);
                int cost = 0;
                cost = CostCalculator.CalculateCost((int)_currentProductData.MinCost, _sizes);
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
        private void SerializeProductSizes(ISizeContainer sizes, ISizeContainer product)
        {
            product.Height = sizes.Height;
            product.Width = sizes.Width;
            product.Diameter = sizes.Diameter;
            product.Length = sizes.Length;
            product.AdditionalSize = sizes.AdditionalSize;
        }
        private List<string> InsertSizeValues(List<SizeContainer> sizes)
        {
            List<string> allSizes = new();
            foreach (var size in sizes)
                allSizes.Add(SizeHelper.InsertStringSizes(size, 'x'));
            return allSizes;
        }
    }
}
