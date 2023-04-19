using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BasketStoreTelegramBot.Entities;
using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.Features.States;
using BasketStoreTelegramBot.Features.States.Admin;
using BasketStoreTelegramBot.Features.States.Constructor.Sizes;
using BasketStoreTelegramBot.Services.User;
using BasketStoreTelegramBot.StateMachines;
using BasketStoreTelegramBot.States;
using BasketStoreTelegramBot.States.Constructor;
using BasketStoreTelegramBot.States.Payment;
using Telegram.Bot.Types;

namespace BasketStoreTelegramBot
{
    public class StateMachine : IStateMachine
    {
        private UserService _userService;
        public StateMachine() { 
            _userService = new UserService();
        }
        public void SetState(string id, IState state)
        {
            UserEntity user = _userService.GetValueByChatId(id);
            user.CurrentState = (int)state.StateType;
            user.State = state;
            _userService.Update(user);
        }
        public async Task<IMessage> FireEvent(MessageEvent data, Update update)
        {
            if (_userService.TryGetValue(data.Id, out var currentState))
            {
                if (_userService.GetValueByChatId(data.Id).State == null)
                    currentState = new InitState(this);
                return await currentState.Update(data);
            }
            var state = new InitState(this);
            SetState(data.Id, state);
            return await state.Update(data);
        }
        public void GetLastActiveState(Update update)
        {
            UserEntity user = _userService.GetValueByChatId(update.Message.Chat.Id.ToString());
            user.State = GetState(_userService.GetValue(update).CurrentState);
        }
        public IState GetState(int stateNumber)
        {
            switch ((StateTypes)stateNumber) {
                case StateTypes.Init: return new InitState(this);
                case StateTypes.ShoppingBagState: return new ShoppingBagState(this);
                case StateTypes.ProductTypeSelector: return new ProductTypeSelector(this);
                case StateTypes.CharacteristicsChanger: return new ChracteristicsChanger(this);
                case StateTypes.BottomTypeSelector: return new BottomTypeSelector(this);
                case StateTypes.ColorSelector: return new ColorSelector(this);
                case StateTypes.ChooseSizeType: return new ChooseSizeType(this);
                case StateTypes.SpecificsSelection: return new SpecificsSelection(this);
                case StateTypes.ConstructorEnd: return new ConstructorEnd(this);
                case StateTypes.DeliveryAddress: return new DeliveryAddress(this);
                case StateTypes.PaymentState: return new PaymentState(this);
                case StateTypes.RecieverInfo: return new RecieverInfo(this);
                case StateTypes.AdminInitState: return new AdminInitState(this, new InitState(this));
                case StateTypes.AdminControlsState: return new AdminControlsState(this);
                case StateTypes.UserNotificationState: return new UserNotificationState(this);
                case StateTypes.CatalogState: return new CatalogState(this);
                case StateTypes.OwnSizeCreator: return new OwnSizeCreator(this);
                case StateTypes.ExistingSizeSelector: return new ExistingSizeSelector(this);
                default: throw new ArgumentOutOfRangeException(nameof(stateNumber));
            }
        }
    }
}