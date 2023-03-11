using BasketStoreTelegramBot.Comands;
using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.StateMachines;
using System.Threading.Tasks;
using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.Features.ProductInformation;

namespace BasketStoreTelegramBot.States
{
    class ColorSelector : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IState? _returnState;
        private static ProductData currentProduct = ProductDataJsonDeserializer.Instance.
                                                    CurrentProductInfo(ShoppingBag.Instance.CurrentProduct.Name);
        private readonly string _colors = currentProduct.Palette;

        public StateTypes StateType => StateTypes.ColorSelector;

        public ColorSelector(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        public ColorSelector(IStateMachine stateMachine, IState returnState)
        {
            _stateMachine = stateMachine;
            _returnState = returnState;
        }


        public async Task<IMessage> Initialize(MessageEvent data)
        {
            return new PhotoMessage()
            {
                Caption = "Выберите необходимый цвет",
                Link = currentProduct.PalettePhoto,
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
            if (_colors.Contains(text))
            {
                ShoppingBag.Instance.CurrentProduct.Color = text;
                IState state;
                if (currentProduct.Sizes.Count != 0)
                    state = new SizeSelector(_stateMachine);
                else if (currentProduct.Specifics.Count != 0)
                    state = new SpecificsSelection(_stateMachine);
                else
                    state = new ConstructorEnd(_stateMachine);
                if (_returnState != null)
                    state = _returnState;

                _stateMachine.SetState(data.Id, state);
                return await state.Initialize(data);
            }
            return new TextMessage()
            {
                Text = "Команда не опознана! Повторите ввод"
            };
        }
        
    }
}
