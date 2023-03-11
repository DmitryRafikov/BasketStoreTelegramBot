using BasketStoreTelegramBot.States;
using BasketStoreTelegramBot.Services.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.StateMachines;

namespace BasketStoreTelegramBot.Features.States.Admin
{
    class AdminInitState : IState
    {
        public StateTypes StateType => StateTypes.AdminInitState;
        private IState _returnState;
        private IStateMachine _stateMachine;
        private IAdminService _adminState = new AdminService();

        public AdminInitState(IStateMachine stateMachine, IState returnState)
        {
            _stateMachine = stateMachine;
            _returnState = returnState;
        }
        public async Task<IMessage> Initialize(MessageEvent data)
        {
            IState state;
            if (_adminState.AdminExists(data))
            {
                state = new AdminControlsState(_stateMachine);
                _stateMachine.SetState(data.Message, state);
                await state.Initialize(data);
                
            }
            state = _returnState;
            _stateMachine.SetState(data.Message, state);
            return await Update(data);
        }

        public async Task<IMessage> Update(MessageEvent data)
        {
            return new TextMessage
            {
                Text = "У вас недостаточно прав для использования панели администратора!"
            };
        }
    }
}
