﻿using BasketStoreTelegramBot.MessagesHandle;
using BasketStoreTelegramBot.StateMachines;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using BasketStoreTelegramBot.Features;
using BasketStoreTelegramBot.Features.ProductInformation;
using BasketStoreTelegramBot.Comands;

namespace BasketStoreTelegramBot.States
{
    class ConstructorEnd : IState
    {
        private readonly IStateMachine _stateMachine;
        private ProductData _currentProductInfo;
        private ShoppingBag _shoppingBag;
        private readonly List<string> _buttons = new List<string>() {
            "Изменить",
            "Добавить в отложенные",
            "Вернуться в начало"
        };

        public StateTypes StateType => StateTypes.ConstructorEnd;

        public ConstructorEnd(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _shoppingBag = new ShoppingBag();
        }
        public async Task<IMessage> Initialize(MessageEvent data)
        {
            int chatID = Convert.ToInt32(data.Id);
            _currentProductInfo = GetProductData(chatID);
            var product = _shoppingBag.CurrentProduct(chatID);
            product.PhotoLink = _currentProductInfo.Photo;
            await _shoppingBag.UpdateInfo(product);
            _shoppingBag.AddProductInBag(chatID);
            var keyboard = new Markup()
            {
                KeyboardWithText = _buttons
            };
            return new PhotoMessage()
            {
                Link = _currentProductInfo.Photo,
                ReplyMarkup = keyboard.Insert(),
                Caption = product.ToString(),
            };
            
        }

        public async Task<IMessage> Update(MessageEvent data)
        {
            var text = data.Message.ToLower();
            if (CommandsList.AllCommands.Contains(data.Message.ToLower()))
            {
                CommandExecutor action = new CommandExecutor(_stateMachine);
                return await action.DefineCommand(data.Message.ToLower(), data);
            }
            if (_buttons.Contains(data.Message))
            {
                if (text == "добавить в отложенные")
                {
                    return new TextMessage
                    {
                        Text = "Добавлено в отложенные"
                    };
                }
                IState state = null;
                if (text == "вернуться в начало")
                {
                    state = new InitState(_stateMachine);
                }
                if (text == "изменить")
                {
                    state = new ChracteristicsChanger(_stateMachine);
                }
                _stateMachine.SetState(data.Id, state);
                return await state.Initialize(data);
            }
            var keyboard = new Markup()
            {
                KeyboardWithText = _buttons
            };
            return new TextMessage { 
                Text = "Не получилось опознать команду. Пожалуйста, выберите действие из предложенных",
                ReplyMarkup = keyboard.Insert()
            };
        }
        private ProductData GetProductData(int chatID)
        {
            var product = _shoppingBag.CurrentProduct(chatID);
            var name = product.Name;
            var bottom = product.BottomType;
            if (bottom != null)
                return ProductDataJsonDeserializer.Instance.CurrentProductInfo(name, bottom);
            return ProductDataJsonDeserializer.Instance.CurrentProductInfo(name);
        }
    }
}
