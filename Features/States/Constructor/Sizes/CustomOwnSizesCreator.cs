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
    internal class CustomOwnSizesCreator : SizeCreatorHelper, IState
    {
        public StateTypes StateType => StateTypes.CustomOwnSizesCreator;
        public CustomOwnSizesCreator(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _shoppingBag = new ShoppingBag();
        }
        public CustomOwnSizesCreator(IStateMachine stateMachine, IState returnState)
        {
            _stateMachine = stateMachine;
            _returnState = returnState;
            _shoppingBag = new ShoppingBag();
        }

        public async Task<IMessage> Initialize(MessageEvent data)
        {
            int chatID = Convert.ToInt32(data.Id);
            var productData = GetProductData(chatID);
            var format = productData.Format;
            var keyboard = new Markup()
            {
                KeyboardWithText = InsertSizeValues(productData.Sizes)
            };
            return new TextMessage()
            {
                Text = "Напишите необходимый вам собственный размер или выберите из предложенных" +
                "\nРазмеры указаны в сантиметрах в формате " + format,
                ReplyMarkup = keyboard.Insert()
            };
        }

        public async Task<IMessage> Update(MessageEvent data)
        {
            int chatID = Convert.ToInt32(data.Id);
            var productData = GetProductData(chatID);
            var format = productData.Format;
            var text = data.Message.ToLower();
            var product = _shoppingBag.CurrentProduct(chatID);

            if (CommandsList.AllCommands.Contains(text))
            {
                CommandExecutor action = new CommandExecutor(_stateMachine);
                return await action.DefineCommand(text, data);
            }
            try
            {
                _sizes = SizeHelper.ToSizeContainer(format, text);
                ISizeContainer enteredSizeContainer = SizeHelper.HasValue(_sizes) ? _sizes : SizeHelper.ToSizeContainer(format, text);
                int cost = 0;
                cost = CostCalculator.CalculateCost((int)productData.MinCost, _sizes);
                SerializeSizesInProduct(_sizes, product);
                product.Cost = cost;
                return await ChangeState(data, product);
            }
            catch
            {
                return new TextMessage()
                {
                    Text = "Не получилось преобразовать размер в необходимый формат."+
                    "Проверьте корректность введенных данных и повторите ввод"+
                    "Данные должны быть введены в формате: " + format
                };
            }
        }
    }
}
