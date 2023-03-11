using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.StateMachines;
using BasketStoreTelegramBot.States;

namespace BasketStoreTelegramBot
{
    public class StateMachine : IStateMachine
    {
        private readonly Dictionary<string, IState> _stateStorage;
        private readonly Func<IState> _initStateFactory;

        public StateMachine(Func<IState> initStateFactory)
        {
            _stateStorage = new Dictionary<string, IState>();
            _initStateFactory = initStateFactory;
        }
        public void SetState(string id, IState state)
        {
            _stateStorage[id] = state;//для перевода клиента в бд в другое состояние
        }
        public async Task<IMessage> FireEvent(MessageEvent data)
        {
            if (_stateStorage.TryGetValue(data.Id, out var currentState))
            {
                return await currentState.Update(data);
            }
            var state = _initStateFactory();
            SetState(data.Id, state);
            return await state.Update(data);
        }
    }
}