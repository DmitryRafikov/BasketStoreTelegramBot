using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Features.ProductInformation
{
    class ProductDataJsonDeserializer
    {
        private static readonly Lazy<ProductDataJsonDeserializer> _productsData = new Lazy<ProductDataJsonDeserializer>(() => new ProductDataJsonDeserializer());
        public static ProductDataJsonDeserializer Instance { get { return _productsData.Value; } }
        private List<ProductData> _productsInfo;
        private ProductDataJsonDeserializer()
        {
            DeserializeAllData();
        }
        private void DeserializeAllData()
        {
            string path = @"C:\Users\vladu\source\repos\BasketStoreTelegramBot\Data\ProductTypesData.json";
            _productsInfo = JsonDataSerializer<ProductData>.GetDeserializedObjectList(path, _productsInfo);
        }
        public ProductData CurrentProductInfo(string productName)
        {
            return _productsInfo
                    .Where(e => e.Name.ToLower()
                             == productName.ToLower())
                    .FirstOrDefault();
        }
        public ProductData CurrentProductInfo(string productName, string type)
        {
            return _productsInfo
                    .Where(e => e.Name.ToLower() == productName.ToLower() && e.Type.ToLower() == type.ToLower())
                    .FirstOrDefault();
        }
        public List<string> GetNames()
        {
            return new List<string>(_productsInfo.Select(e => e.Name).Distinct());
        }
        public List<string> GetTypes(string name)
        {
            if (GetNames().Contains(name))
                return new List<string>(_productsInfo
                                        .Where(e => e.Name == name)
                                        .Select(e => e.Type)
                                        .Distinct());
            throw new ArgumentException();
        }
    }
}
