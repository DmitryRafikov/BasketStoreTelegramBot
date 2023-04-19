using BasketStoreTelegramBot.Comands;
using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.StateMachines;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.States
{
    class RecieverInfo : IState
    {
        IStateMachine _stateMachine;

        public RecieverInfo(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public StateTypes StateType => StateTypes.RecieverInfo;

        public async Task<IMessage> Initialize(MessageEvent data)
        {
            return new TextMessage { 
                Text = "Введите ваше ФИО без сокращений.\n" +
                        "Это необходимо для обозначения получателя посылки"
            };
        }

        public async Task<IMessage> Update(MessageEvent data)
        {
            if (CommandsList.AllCommands.Contains(data.Message.ToLower()))
            {
                CommandExecutor action = new CommandExecutor(_stateMachine);
                return await action.DefineCommand(data.Message.ToLower(), data);
            }
            var state = new DeliveryAddress(_stateMachine);
            return await state.Initialize(data);
        }
    }
}
