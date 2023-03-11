using BasketStoreTelegramBot.Entities;
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
        private static ProductData _currentProductInfo = ProductDataJsonDeserializer.Instance.
                                   CurrentProductInfo(ShoppingBag.Instance.CurrentProduct.Name,
                                                        ShoppingBag.Instance.CurrentProduct.BottomType);
        private readonly List<string> _buttons = new List<string>() {
            "Изменить",
            "Удалить"
        };
        public StateTypes StateType => StateTypes.ShoppingBagState;
        public ShoppingBagState(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public async Task<IMessage> Initialize(MessageEvent data)
        {
            return new PhotoMessagesCollection() {
                Collection = CreatePhotoMessagesCollection()
            };

        }
        public async Task<IMessage> Update(MessageEvent data)
        {
            if (data.Callback != null)
            {
                if (data.Callback.Message.Text.ToLower() == "удалить")
                {
                    ShoppingBag.Instance.RemoveProductByIndex(Convert.ToInt32(data.Callback.Data));
                    return new TextMessage()
                    {
                        Text = "Выбранный товар удален из списка товаров"
                    };
                }
                if (data.Callback.Message.Text.ToLower() == "изменить")
                {
                    ShoppingBag.Instance.RemoveProductByIndex(Convert.ToInt32(data.Callback.Data));
                }
                return new TextMessage()
                {
                    Text = "Действие не опознано"
                };

            }
            if (data.Message.ToLower() == "оформить заказ")
            {
                var state = new DeliveryAddress(_stateMachine);
                return await state.Initialize(data);
            }
            return new TextMessage()
            {
                Text = "Действие не опознано"
            };
        }
        private List<PhotoMessage> CreatePhotoMessagesCollection() {
            var list = new List<PhotoMessage>();
            foreach (var item in ShoppingBag.Instance.Products) {
                var keyboard = new Markup
                {
                    KeyboardWithCallBack = GetItemActionsCollection(ShoppingBag.Instance.Products.IndexOf(item))
                };
                var message = new PhotoMessage()
                {
                    Link = ProductDataJsonDeserializer.Instance.
                                   CurrentProductInfo(item.Name, item.BottomType).Photo,
                    Caption = item.ToString(),
                    ReplyMarkup = keyboard.Insert()
                };
                list.Add(message);
            }
            return list;
        }
        private Dictionary<string, string> GetItemActionsCollection(int itemNumber)
        {
            return new Dictionary<string, string>() {
                { "Изменить", itemNumber.ToString()},
                { "Удалить", itemNumber.ToString()}
            };
        }
    }
}
