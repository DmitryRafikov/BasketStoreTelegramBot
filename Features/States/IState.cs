using BasketStoreTelegramBot.Features;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.States
{
    public interface IState
    {
        StateTypes StateType { get; }
        Task<IMessage> Update(MessageEvent data);
        Task<IMessage> Initialize(MessageEvent data);
    }
}
