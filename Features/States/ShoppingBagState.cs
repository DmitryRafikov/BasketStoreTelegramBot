using BasketStoreTelegramBot.Comands;
using BasketStoreTelegramBot.Entities.Products;
using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.Features.ProductInformation;
using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.StateMachines;
using System;
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
            List<ProductEntity> products = (List<ProductEntity>)_shoppingBag.GetAddedProducts(chatID);
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
                if (data.Callback.Message.Text.ToLower() == "удалить")
                {
                    int chatID = Convert.ToInt32(data.Id);
                    List<ProductEntity> products = (List<ProductEntity>)_shoppingBag.GetAddedProducts(chatID);
                    var productToRemove = products[Convert.ToInt32(data.Callback.Data)];
                    _shoppingBag.RemoveProduct(chatID, productToRemove);
                    return new TextMessage()
                    {
                        Text = "Выбранный товар удален из списка товаров"
                    };
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
            List<ProductEntity> products = (List<ProductEntity>)_shoppingBag.GetAddedProducts(chatID);
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
