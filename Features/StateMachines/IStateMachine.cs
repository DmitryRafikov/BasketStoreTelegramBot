using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.States;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace BasketStoreTelegramBot.StateMachines
{
    public interface IStateMachine
    {
        Task<IMessage> FireEvent(MessageEvent data, Update update);
        void SetState(string id, IState state);
        void GetLastActiveState(Update update);
        IState GetState(int stateNumber);
    }
}
