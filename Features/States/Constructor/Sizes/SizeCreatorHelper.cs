using BasketStoreTelegramBot.Entities.Products;
using BasketStoreTelegramBot.Features.ProductInformation;
using BasketStoreTelegramBot.StateMachines;
using BasketStoreTelegramBot.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Features.States.Constructor.Sizes
{
    class SizeCreatorHelper
    {
        public IStateMachine _stateMachine;
        protected IState _returnState;
        protected SizeContainer _sizes;
        protected ShoppingBag _shoppingBag;
        protected async Task<IMessage> ChangeState(MessageEvent data, ProductEntity product) {
            var chatID = Convert.ToInt32(data.Id);
            var productData = GetProductData(chatID);
            IState state;
            bool hasSpecifics;
            if (product.Diameter.HasValue)
                hasSpecifics = productData.GetSpecifics(product.Diameter.Value).Count == 0;
            else
                hasSpecifics = false;

            if (GetProductData(chatID).Specifics == null && !hasSpecifics)
                state = new ConstructorEnd(_stateMachine);
            else
                state = new SpecificsSelection(_stateMachine);
            if (_returnState != null)
                state = _returnState;
            await _shoppingBag.UpdateInfo(product);
            _stateMachine.SetState(data.Id, state);
            return await state.Initialize(data);
        }
        protected ProductData GetProductData(int chatID)
        {
            var product = _shoppingBag.CurrentProduct(chatID);
            var name = product.Name;
            var bottom = product.BottomType;
            if (bottom != null)
                return ProductDataJsonDeserializer.Instance.CurrentProductInfo(name, bottom);
            return ProductDataJsonDeserializer.Instance.CurrentProductInfo(name);
        }
        protected void SerializeSizesInProduct(ISizeContainer sizes, ISizeContainer product)
        {
            product.Height = sizes.Height;
            product.Width = sizes.Width;
            product.Diameter = sizes.Diameter;
            product.Length = sizes.Length;
            product.AdditionalSize = sizes.AdditionalSize;
        }
        protected List<string> InsertSizeValues(List<SizeContainer> sizes)
        {
            List<string> allSizes = new();
            foreach (var size in sizes)
                allSizes.Add(SizeHelper.InsertStringSizes(size, 'x'));
            return allSizes;
        }
    }
}
