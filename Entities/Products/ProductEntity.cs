using BasketStoreTelegramBot.Features.ProductInformation;

namespace BasketStoreTelegramBot.Entities.Products
{
    class ProductEntity : IProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string BottomType { get; set; }
        public int? Height { get; set; }
        public int? Width { get; set; }
        public int? Length { get; set; }
        public int? Diameter { get; set; }
        public int? AdditionalSize { get; set; }
        public int? Cost { get; set; }
        public string PhotoLink { get; set; }
        public string Description { get; set; }
        public ProductEntity ToProductEntity(CatalogProductEntity catalogProductEntity)
        {
            Name = catalogProductEntity.Name;
            Color = catalogProductEntity.Color;
            Height = catalogProductEntity.Height;
            Width = catalogProductEntity.Width;
            Diameter = catalogProductEntity.Diameter;
            Length = catalogProductEntity.Length;
            AdditionalSize = catalogProductEntity.AdditionalSize;
            Cost = catalogProductEntity.Cost;
            PhotoLink = catalogProductEntity.PhotoLink;
            Description = catalogProductEntity.Description;
            return this;
        }
        public override string ToString()
        {
            return $"{Name}\n" +
                   $"Цвет: {Color}\n" +
                   ConvertSizes() +
                   $"\nЦена: {Cost}\n{Description}";
        }
        protected string ConvertSizes()
        {
            return (Height != null ? "Высота: " + Height.ToString() + " " : string.Empty) +
                   (Length != null ? "Длина: " + Length.ToString() + " " : string.Empty) +
                   (Width != null ? "Ширина: " + Width.ToString() + " " : string.Empty) +
                   (Diameter != null ? "Диаметр: " + Diameter.ToString() + " " : string.Empty) +
                   (AdditionalSize != null ? "Дополнительно: " + AdditionalSize.ToString() : string.Empty);
        }

    }
}
