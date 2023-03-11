using BasketStoreTelegramBot.Entities;
using BasketStoreTelegramBot.Features.ProductInformation;
using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.Services.CatalogProduct;
using BasketStoreTelegramBot.StateMachines;
using BasketStoreTelegramBot.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Features.States
{
    class CatalogState : IState
    {
        public StateTypes StateType => StateTypes.CatalogState;

        IStateMachine _stateMachine;
        private static ICatalogProductService _catalogProductService = CatalogProductService.Instance;
        private readonly List<string> _buttons = new List<string>() {
            "Изменить",
            "Удалить"
        };
        public CatalogState(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public async Task<IMessage> Initialize(MessageEvent data)
        {
            return new PhotoMessagesCollection()
            {
                Collection = CreatePhotoMessagesCollection()
            };

        }
        public async Task<IMessage> Update(MessageEvent data)
        {
            if (data.Callback != null)
            {
                if (data.Callback.Message.Text.ToLower() == "добавить в отложенные")
                {
                    ShoppingBag.Instance.AddProductInBag(
                        CatalogProductEntity.ToProductEntity(
                            _catalogProductService.GetProduct(Convert.ToInt32(data.Callback.Data)))
                    );
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
        private List<PhotoMessage> CreatePhotoMessagesCollection()
        {
            var list = new List<PhotoMessage>();
            foreach (var item in _catalogProductService.GetAllProducts())
            {
                var keyboard = new Markup
                {
                    KeyboardWithCallBack = GetItemActionsCollection(item.Id)
                };
                var message = new PhotoMessage()
                {
                    Link = item.PhotoLink,
                    Caption = item.ToString(),
                    ReplyMarkup = keyboard.Insert()
                };
                list.Add(message);
            }
            return list;
        }
        private Dictionary<string, string> GetItemActionsCollection(int itemId)
        {
            return new Dictionary<string, string>() {
                { "Добавить в отложенные", itemId.ToString()}
            };
        }
    }
    
}
