using BasketStoreTelegramBot.Comands;
using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.StateMachines;
using BasketStoreTelegramBot.States.Payment;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.States
{
    class DeliveryAddress : IState
    {
        IStateMachine _stateMachine;
        public DeliveryAddress(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;        
        }

        public StateTypes StateType => StateTypes.DeliveryAddress;

        public async Task<IMessage> Initialize(MessageEvent data)
        {
            return new TextMessage
            {
                Text = "Введите адрес доставки с индексом.\n" +
                        "Например: г. Омск, ул. Есенина, д. 126, кв"
            };
        }

        public async Task<IMessage> Update(MessageEvent data)
        {
            if (CommandsList.AllCommands.Contains(data.Message.ToLower()))
            {
                CommandExecutor action = new CommandExecutor(_stateMachine);
                return await action.DefineCommand(data.Message.ToLower(), data);
            }
            var state = new PaymentState(_stateMachine);
            return await state.Initialize(data);
        }
    }
}
