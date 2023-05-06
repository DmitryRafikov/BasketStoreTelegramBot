using BasketStoreTelegramBot.Comands;
using BasketStoreTelegramBot.Entities.Products;
using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.Features.ProductInformation;
using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.Others;
using BasketStoreTelegramBot.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.States
{
    class SpecificsSelection : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IState _returnState;
        private ShoppingBag _shoppingBag;

        public StateTypes StateType => StateTypes.SpecificsSelection;

        public SpecificsSelection(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _shoppingBag = new ShoppingBag();
        }
        public SpecificsSelection(IStateMachine stateMachine, IState returnState)
        {
            _stateMachine = stateMachine;
            _returnState = returnState;
            _shoppingBag = new ShoppingBag();
        }

        public async Task<IMessage> Initialize(MessageEvent data)
        {
            int chatID = Convert.ToInt32(data.Id);
            var keyboard = new Markup()
            {
                KeyboardWithText = DefineButtons(chatID)
            };            
            return new TextMessage()
            {
                Text = "Выберите что необходимо добавить к вашему товару. Чтобы отказаться от добавленного, повторите нажатие",
                ReplyMarkup = keyboard.Insert()
            };
        }

        public async Task<IMessage> Update(MessageEvent data)
        {
            int chatID = Convert.ToInt32(data.Id);
            ProductData productData = GetProductData(chatID, out var product);
            List<string> selected = product.Description == null?
                                                           new ():
                                                           product.Description.Split(", ").ToList();
            var text = data.Message.ToLower();
            var buttons = DefineButtons(chatID);

            var keyboard = new Markup()
            {
                KeyboardWithText = buttons
            };
            if (CommandsList.AllCommands.Contains(text))
            {
                CommandExecutor action = new CommandExecutor(_stateMachine);
                return await action.DefineCommand(text, data);
            }
            if (buttons.Contains(data.Message))
            {
                if (text == "готово" || text == "не имеет особенностей") {
                    if (text == "не имеет особенностей" || selected.Count == 0){
                        selected.Clear();
                        selected.Add(text);
                    }
                    product.Description = ListConverter.ToString(selected);
                    await _shoppingBag.UpdateInfo(product);
                    IState state = new ConstructorEnd(_stateMachine);
                    if (_returnState != null)
                        state = _returnState;
                    _stateMachine.SetState(data.Id, state);
                    return await state.Initialize(data);
                }
                string reply;
                int cost;
                if (!selected.Contains(text)){
                    selected.Add(text);
                    cost = productData.Specifics.First(x => x.Name == data.Message).Cost;
                    reply = "Добавлено";
                }
                else {
                    selected.Remove(text);
                    cost = productData.Specifics.First(x => x.Name == data.Message).Cost *-1;
                    reply = "Удалено";
                }
                product.Description = ListConverter.ToString(selected);
                product.Cost += cost;
                await _shoppingBag.UpdateInfo(product);
                return new TextMessage()
                {
                    Text = reply,
                    ReplyMarkup = keyboard.Insert()
                };

            }
            return new TextMessage()
            {
                Text = "Команда не опознана! Выберите тип товара",
                ReplyMarkup = keyboard.Insert()
            };
        }
        private List<string> DefineButtons(int chatID)
        {
            var productData = GetProductData(chatID, out ProductEntity product);
            var specifics = productData.GetSpecifics(product.Diameter.Value);
            specifics.Add("Не имеет особенностей");
            specifics.Add("Готово");
            return specifics;
        }
        private ProductData GetProductData(int chatID, out ProductEntity product)
        {
            product = _shoppingBag.CurrentProduct(chatID);
            var name = product.Name;
            var bottom = product.BottomType;
            if (bottom != null)
                return ProductDataJsonDeserializer.Instance.CurrentProductInfo(name, bottom);
            return ProductDataJsonDeserializer.Instance.CurrentProductInfo(name);
        }
    }
}
