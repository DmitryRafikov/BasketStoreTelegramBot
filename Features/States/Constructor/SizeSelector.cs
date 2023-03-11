using BasketStoreTelegramBot.Comands;
using BasketStoreTelegramBot.Entities;
using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.Features.ProductInformation;
using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.States
{
    class SizeSelector : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IState _returnState;
        private static ProductData currentProductData = ProductDataJsonDeserializer.Instance.
                                                   CurrentProductInfo(ShoppingBag.Instance.CurrentProduct.Name,
                                                                        ShoppingBag.Instance.CurrentProduct.BottomType);

        private readonly string? format = currentProductData.Format;
        private bool diameterIsChoosen = false;
        private bool heightIsChoosen = false;
        private SizeContainer _sizes;

        public StateTypes StateType => StateTypes.SizeSelector;

        private List<string> Sizes() 
        {
            List<string> allSizes = new();
            foreach (var size in currentProductData.Sizes)
                allSizes.Add(size.InsertStringSizes());
            return allSizes;
        }
        public SizeSelector(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _sizes = new SizeContainer();

        }
        public SizeSelector(IStateMachine stateMachine, IState returnState)
        {
            _stateMachine = stateMachine;
            _returnState = returnState;
        }
        public async Task<IMessage> Initialize(MessageEvent data)
        {
            var keyboard = new Markup()
            {
                KeyboardWithText = Sizes()
            };
            string text = format == string.Empty ? "" : " размер или напишите свой собственный в формате ";
            return new TextMessage()
            {
                Text = "Выберите необходимый вам размер" + text +
                format + ". \nВсе размеры указаны в сантиметрах",
                ReplyMarkup = keyboard.Insert()
            };
        }

        public async Task<IMessage> Update(MessageEvent data)
        {
            var text = data.Message.ToLower();
            if (CommandsList.AllCommands.Contains(text))
            {
                CommandExecutor action = new CommandExecutor(_stateMachine);
                return await action.DefineCommand(text, data);
            }
            try
            {
                var product = ShoppingBag.Instance.CurrentProduct;
                if (currentProductData.Resizable != null)
                {
                    BasketsGeometryTypes geometryType;
                    if (currentProductData.Name.Contains("круг"))
                        geometryType = BasketsGeometryTypes.CIRCLE;
                    else
                        geometryType = BasketsGeometryTypes.RECTANGLE;
                    if (currentProductData.HasConstantSizes!= null)
                    {
                        if (!diameterIsChoosen)
                            return await ChooseExtraSizes();
                        else
                        {
                            if (currentProductData.HasConstantSizes != null)
                                _sizes.Diameter = Convert.ToInt32(text);
                            else
                                throw new ArgumentException();
                            if (!heightIsChoosen)
                            {
                                heightIsChoosen = true;
                                return new TextMessage()
                                {
                                    Text = "Напишите необходимую вам высоту"
                                };
                            }
                            else
                                _sizes.Height = Convert.ToInt32(text);
                        }

                        SerializeProduct(_sizes, product);
                    }
                    else
                        SerializeProduct(new SizeContainer(text, geometryType), product);
                }
                else if (Sizes().Count > 1)
                    SerializeProduct(currentProductData.Sizes
                                                        .Where(e => e.Length == Convert.ToInt32(text))
                                                        .FirstOrDefault(), product);
                else
                    SerializeProduct(currentProductData.Sizes[0], product);
                IState state;
                if (currentProductData.Specifics == null && currentProductData.GetSpecifics(ShoppingBag.Instance.CurrentProduct.Diameter.Value) == null)
                    state = new ConstructorEnd(_stateMachine);
                else
                    state = new SpecificsSelection(_stateMachine);
                if (_returnState != null)
                    state = _returnState;
                _stateMachine.SetState(data.Id, state);
                return await state.Initialize(data);
            }
            catch (ArgumentException) 
            {
                return new TextMessage()
                {
                    Text = "Размер введен неверно. Убедитесь в том что вводите данные в формате: " + format
                };
            }
        }
        private async Task<IMessage> ChooseExtraSizes() 
        {
            diameterIsChoosen = true;
            var keyboard = new Markup()
            {
                KeyboardWithText = Sizes()
            };
            return new TextMessage()
            {
                Text = "Выберите размер диаметра",
                ReplyMarkup = keyboard.Insert()
            };
        }
        private void SerializeProduct(SizeContainer sizes, ProductEntity product) {
            product.Height = sizes.Height;
            product.Width = sizes.Width;
            product.Diameter = sizes.Diameter;
            product.Length = sizes.Length;
            product.AddidionalSize = sizes.Handles;
        }
    }
}
