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
    internal class ConstantOwnSizesCreator :SizeCreatorHelper, IState
    {
        public StateTypes StateType => StateTypes.ConstantOwnSizesCreator;
        public ConstantOwnSizesCreator(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _shoppingBag = new ShoppingBag();
            _sizes = new SizeContainer();
        }
        public ConstantOwnSizesCreator(IStateMachine stateMachine, IState returnState)
        {
            _stateMachine = stateMachine;
            _returnState = returnState;
            _shoppingBag = new ShoppingBag();
            _sizes = new SizeContainer();
        }

        public async Task<IMessage> Initialize(MessageEvent data)
        {
            int chatID = Convert.ToInt32(data.Id);
            var productData = GetProductData(chatID);
            var format = productData.Format.Replace("ВЫСОТАх", "");
            var keyboard = new Markup()
            {
                KeyboardWithText = InsertSizeValues(productData.ConstantSizes)
            };
            return new TextMessage()
            {
                Text = "Выберите необходимый вам диметр корзины" +
                "\nРазмеры указаны в сантиметрах в формате " + format,
                ReplyMarkup = keyboard.Insert()
            };
        }
        public async Task<IMessage> Update(MessageEvent data)
        {
            int chatID = Convert.ToInt32(data.Id);
            var productData = GetProductData(chatID);
            var format = productData.Format.Replace("ВЫСОТАх", "");
            var text = data.Message.ToLower();
            var product = _shoppingBag.CurrentProduct(chatID);
            SerializeSizesInProduct(product, _sizes);
            if (CommandsList.AllCommands.Contains(text))
            {
                CommandExecutor action = new CommandExecutor(_stateMachine);
                return await action.DefineCommand(text, data);
            }            
            try{
                if (!_sizes.HasValue)
                {
                    if (InsertSizeValues(productData.ConstantSizes).Contains(text))
                    {
                        _sizes = SizeHelper.ToSizeContainer(format, text);
                        SerializeSizesInProduct(_sizes, product);
                        await _shoppingBag.UpdateInfo(product);
                        return new TextMessage()
                        {
                            Text = "Напишите необходимую вам высоту корзины",
                        };
                    }
                    else throw new ArgumentException();
                }
                _sizes.Height = Convert.ToInt32(text);
                ISizeContainer enteredSizeContainer = SizeHelper.HasValue(_sizes) ? _sizes : SizeHelper.ToSizeContainer(format, text);

                int cost = 0;
                cost = CostCalculator.CalculateCost((int)productData.MinCost, _sizes);
                SerializeSizesInProduct(_sizes, product);
                product.Cost = cost;
                return await ChangeState(data, product);
            }
            catch
            {
                var keyboard = new Markup()
                {
                    KeyboardWithText = InsertSizeValues(productData.ConstantSizes)
                };
                return new TextMessage()
                {
                    Text = "Пожалуйста, выберите размер из предложенных или повторите ввод",
                    ReplyMarkup = keyboard.Insert()
                };
            }
            
        }
    }
}
