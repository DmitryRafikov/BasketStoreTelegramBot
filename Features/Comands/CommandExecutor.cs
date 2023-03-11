using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.StateMachines;
using BasketStoreTelegramBot.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Comands
{
    class CommandExecutor
    {
        public IStateMachine _stateMachine;
        public CommandExecutor(IStateMachine stateMachine) {
            _stateMachine = stateMachine;
        }
        public Task<IMessage> DefineCommand(string commandName, MessageEvent data) {
            switch (commandName) {
                case (CommandsList.StartCommand):
                    return GetState(_stateMachine, new InitState(_stateMachine), data);
                case (CommandsList.ConstructorCommand):
                    return GetState(_stateMachine, new ProductTypeSelector(_stateMachine), data);
                case (CommandsList.ShowDelayed):
                    return GetState(_stateMachine, new ShoppingBagState(_stateMachine), data);
                case (CommandsList.ShowDeliveryInfo):
                    return GetState(_stateMachine, new InitState(_stateMachine), data);
                case (CommandsList.ChangeToAdmin):
                    return GetState(_stateMachine, new InitState(_stateMachine), data);
                default:throw new ArgumentException();
            }
        }
        private Task<IMessage> GetState(IStateMachine _stateMachine,IState state, MessageEvent data)
        {
            _stateMachine.SetState(data.Id, state);
            return state.Initialize(data);
        }
    }
}
 