using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.States;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.StateMachines
{
    public interface IStateMachine
    {
        Task<IMessage> FireEvent(MessageEvent data);
        void SetState(string id, IState state);
    }
}
