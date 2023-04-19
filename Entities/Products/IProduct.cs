using BasketStoreTelegramBot.Features.ProductInformation;

namespace BasketStoreTelegramBot.Entities.Products
{
    internal interface IProduct:ISizeContainer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int? Height { get; set; }
        public int? Width { get; set; }
        public int? Length { get; set; }
        public int? Diameter { get; set; }
        public int? AdditionalSize { get; set; }
        public int? Cost { get; set; }
        public string PhotoLink { get; set; }
        public string Description { get; set; }
        public string BottomType { get; set; }
    }
}
