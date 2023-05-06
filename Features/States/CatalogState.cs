using BasketStoreTelegramBot.Comands;
using BasketStoreTelegramBot.Entities.Products;
using BasketStoreTelegramBot.Features.ProductInformation;
using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.Models;
using BasketStoreTelegramBot.Services.Product;
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
        private ProductService _catalogProductService;
        private ShoppingBag _shoppingBag;
        private readonly List<string> _buttons = new List<string>() {
            "Изменить",
            "Удалить"
        };
        public CatalogState(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _shoppingBag = new ShoppingBag();
            _catalogProductService = new ProductService();
        }

        public async Task<IMessage> Initialize(MessageEvent data)
        {
            var products = (List<ProductEntity>)_catalogProductService.GetAll().Where(x => x.IsCatalogProduct.Value);
            if (products.Count != 0)
                return new PhotoMessagesCollection()
                {
                    Collection = CreatePhotoMessagesCollection()
                };
            else
                return new TextMessage
                {
                    Text = "тут пока нет товаров"
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
                if (data.Callback.Message.Text.ToLower() == "добавить в отложенные")
                {
                    ProductEntity catalogProduct = _catalogProductService.GetProduct(Convert.ToInt32(data.Callback.Data));
                    _shoppingBag.AddProductInBag(Convert.ToInt32(data.Id), catalogProduct);
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
            var products = (List<ProductEntity>)_catalogProductService.GetAll().Where(x => x.IsCatalogProduct.Value);
            foreach (var item in products)
            {
                var keyboard = new Markup
                {
                    KeyboardWithCallBack = GetItemActionsCollection(item.Id)
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
        private Dictionary<string, string> GetItemActionsCollection(int itemId)
        {
            return new Dictionary<string, string>() {
                { "Добавить в отложенные", itemId.ToString()}
            };
        }
    }
    
}
