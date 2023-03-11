using BasketStoreTelegramBot.Comands;
using BasketStoreTelegramBot.Entities;
using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.Features.ProductInformation;
using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.Others;
using BasketStoreTelegramBot.Services.Product;
using BasketStoreTelegramBot.StateMachines;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.States
{
    class SpecificsSelection : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IState _returnState;
        private static ProductEntity currentProduct = ShoppingBag.Instance.CurrentProduct;
        private static ProductData currentProductInfo = ProductDataJsonDeserializer.Instance.
                                            CurrentProductInfo(ShoppingBag.Instance.CurrentProduct.Name,
                                                                 ShoppingBag.Instance.CurrentProduct.BottomType);
        private List<string> Specifics()
        {
            return currentProductInfo.GetSpecifics(currentProduct.Diameter.Value);
        }
        public List<string> Selected { get; private set; }

        public StateTypes StateType => StateTypes.SpecificsSelection;

        public SpecificsSelection(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            Selected = new List<string>();
        }
        public SpecificsSelection(IStateMachine stateMachine, IState returnState)
        {
            _stateMachine = stateMachine;
            _returnState = returnState;
            Selected = new List<string>();
        }

        public async Task<IMessage> Initialize(MessageEvent data)
        {
            var keyboard = new Markup()
            {
                KeyboardWithText = DefineButtons()
            };            
            return new TextMessage()
            {
                Text = "Выберите что необходимо добавить к вашему товару. Чтобы отказаться от добавленного, повторите нажатие",
                ReplyMarkup = keyboard.Insert()
            };
        }
        private List<string> DefineButtons()
        {
            var specifics = Specifics();
            specifics.Add("Не имеет особенностей");
            specifics.Add("Готово");
            return specifics;
        }

        public async Task<IMessage> Update(MessageEvent data)
        {
            var text = data.Message.ToLower();
            var keyboard = new Markup()
            {
                KeyboardWithText = DefineButtons()
            };
            if (CommandsList.AllCommands.Contains(text))
            {
                CommandExecutor action = new CommandExecutor(_stateMachine);
                return await action.DefineCommand(text, data);
            }
            if (DefineButtons().Contains(data.Message))
            {
                if (text == "готово" || text == "не имеет особенностей") {
                    if (text == "не имеет особенностей" || Selected.Count == 0){
                        Selected.Clear();
                        Selected.Add(text);
                    }
                    ShoppingBag.Instance.CurrentProduct.Specifics = ListConverter.ToString(Selected);
                    IState state = new ConstructorEnd(_stateMachine);
                    if (_returnState != null)
                        state = _returnState;
                    _stateMachine.SetState(data.Id, state);
                    return await state.Initialize(data);
                }
                string reply;
                if (!Selected.Contains(text)){
                    Selected.Add(text);
                    reply = "Добавлено";
                }
                else {
                    Selected.Remove(text);
                    reply = "Удалено";
                }
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
    }
}
