using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.StateMachines;
using BasketStoreTelegramBot.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Features.States.Admin
{
    class AdminControlsState : IState
    {
        public StateTypes StateType => StateTypes.AdminControlsState;
        private IStateMachine _stateMachine;
        private List<string> _buttons = new List<string>()
        {
            "Уведомить о начале работ"
        };

        public AdminControlsState(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public async Task<IMessage> Initialize(MessageEvent data)
        {
            return new TextMessage
            {
                Text = "Доступ разрешен! Выберите действие"
            };
        }

        public async Task<IMessage> Update(MessageEvent data)
        {
            var keyboard = new Markup()
            {
                KeyboardWithText = _buttons
            };
            if (_buttons.Contains(data.Message))
            {
                var text = data.Message.ToLower();
                IState state;
                if (text == "уведомить о начале работ") {
                    state = new UserNotificationState(_stateMachine);
                    _stateMachine.SetState(data.Id, state);
                    await state.Initialize(data);
                }
            }
            return new TextMessage
            {
                Text = "Комнада не опознана",
                ReplyMarkup = keyboard.Insert()
            };
        }
    }
}
