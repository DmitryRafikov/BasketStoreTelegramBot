using BasketStoreTelegramBot.Features.ProductInformation.Structure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BasketStoreTelegramBot.Features.ProductInformation
{
    [Serializable]
    struct ProductData
    {
        [JsonProperty("name")] public string Name { get; private set; }
        [JsonProperty("format")] public string Format { get; private set; }
        [JsonProperty("type")] public string Type { get; private set; }
        [JsonProperty("photo")] public string Photo { get; private set; }
        [JsonProperty("palette")] public string Palette { get; private set; }
        [JsonProperty("palettePhoto")] public string PalettePhoto { get; private set; }
        [JsonProperty("sizes")] public List<SizeContainer> Sizes { get; private set; }
        [JsonProperty("hasConstantSizes")] public bool? HasConstantSizes { get; private set; }
        [JsonProperty("resizable")] public bool? Resizable { get; private set; }
        [JsonProperty("specifics")] public List<Specific> Specifics { get; private set; }
        [JsonProperty("min-cost")] public double MinCost { get; private set; }
        public List<string> GetSpecifics(int size)
        {
            return Specifics.Where(e => size >= e.sizeDepending.Min || e.sizeDepending.Min == 0
                                && (size <= e.sizeDepending.Max || e.sizeDepending.Min == 0))
                                                        .Select(e => e.Name).ToList();
        }

    }
}
