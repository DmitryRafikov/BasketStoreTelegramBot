using BasketStoreTelegramBot.Comands;
using BasketStoreTelegramBot.Entities.Products;
using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.Features.ProductInformation;
using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.StateMachines;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.States
{
    class ShoppingBagState : IState
    {
        IStateMachine _stateMachine;
        private ShoppingBag _shoppingBag;
        public StateTypes StateType => StateTypes.ShoppingBagState;
        public ShoppingBagState(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _shoppingBag = new ShoppingBag();
        }

        public async Task<IMessage> Initialize(MessageEvent data)
        {
            int chatID = Convert.ToInt32(data.Id);
            List<ProductEntity> products = _shoppingBag.GetAddedProducts(chatID).ToList();
            if (products.Count != 0)
                return new PhotoMessagesCollection()
                {
                    Collection = CreatePhotoMessagesCollection(chatID)
                };
            else
                return new TextMessage
                {
                    Text = "В корзине пока нет товаров!"
                };
        }
        public async Task<IMessage> Update(MessageEvent data)
        {
            
            if (CommandsList.AllCommands.Contains(data.Message.ToLower()))
            {
                CommandExecutor action = new CommandExecutor(_stateMachine);
                return await action.DefineCommand(data.Message.ToLower(), data);
            }
            if (data.Callback != null)
            {
                var callbackText = data.Callback.Message.ReplyMarkup.InlineKeyboard.ToList();
                if (callbackText[Convert.ToInt32(data.Callback.Data)-1].ToList()[Convert.ToInt32(data.Callback.Data)-1].Text.ToLower() == "удалить")
                {
                    int chatID = Convert.ToInt32(data.Id);
                    List<ProductEntity> products = _shoppingBag.GetAddedProducts(chatID).ToList();
                    try
                    {
                        var productToRemove = products[Convert.ToInt32(data.Callback.Data)];
                        _shoppingBag.RemoveProduct(chatID, productToRemove);
                        return new TextMessage()
                        {
                            Text = "Выбранный товар удален из списка товаров"
                        };
                    }
                    catch
                    {
                        return new TextMessage()
                        {
                            Text = "Данный товар был удален ранее"
                        };
                    }
                }
                return new TextMessage()
                {
                    Text = "Действие не опознано"
                };

            }
            if (data.Message.ToLower() == "оформить заказ")
            {
                var state = new DeliveryAddress(_stateMachine);
                _stateMachine.SetState(data.Id, state);
                return await state.Initialize(data);
            }
            return new TextMessage()
            {
                Text = "Действие не опознано"
            };
        }
        private List<PhotoMessage> CreatePhotoMessagesCollection(int chatID) {
            var list = new List<PhotoMessage>();
            List<ProductEntity> products = _shoppingBag.GetAddedProducts(chatID).ToList();
            foreach (var item in products) {
                var keyboard = new Markup
                {
                    KeyboardWithCallBack = GetItemActionsCollection(products.IndexOf(item))
                };
                var message = new PhotoMessage()
                {
                    Link = item.PhotoLink,
                    Caption = item.ToString(),
                    ReplyMarkup = keyboard.Insert(true)
                };
                list.Add(message);
            }
            return list;
        }
        private Dictionary<string, string> GetItemActionsCollection(int itemNumber)
        {
            return new Dictionary<string, string>() {
                { "Удалить", itemNumber.ToString()}
            };
        }
    }
}
