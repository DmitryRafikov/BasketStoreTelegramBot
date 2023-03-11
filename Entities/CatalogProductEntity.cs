namespace BasketStoreTelegramBot.Entities
{
    class CatalogProductEntity
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
        public string Descritption { get; set; }

        public static ProductEntity ToProductEntity(CatalogProductEntity _product) {
            return new ProductEntity
            {
                Id = _product.Id,
                Name = _product.Name,
                Color = _product.Color,
                Height = _product.Height,
                Width = _product.Width,
                Diameter = _product.Diameter,
                AddidionalSize = _product.AdditionalSize,
                Cost = _product.Cost,
            };
        }
        public override string ToString()
        {
            return $"{Name}\n" +
                   $"Цвет: {Color}" +
                   ConvertSizes() +
                   $"\nЦена: {Cost}\n{Descritption}";
        }
        private string ConvertSizes() {
            return (Height.HasValue? " Высота: "+  Height.Value.ToString() : string.Empty) +
                   (Length.HasValue? " Длина: " + Length.Value.ToString() : string.Empty) +
                   (Width.HasValue? " Ширина: " + Width.Value.ToString() : string.Empty) +
                   (Diameter.HasValue ? " Диаметр: " + Width.Value.ToString() : string.Empty) +
                   (AdditionalSize.HasValue ? " Дополнительно: " + AdditionalSize.Value.ToString() : string.Empty);
        }

    }
}
