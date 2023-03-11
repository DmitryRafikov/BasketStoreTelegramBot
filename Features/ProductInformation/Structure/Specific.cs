using Newtonsoft.Json;
using System;

namespace BasketStoreTelegramBot.Features.ProductInformation.Structure
{
    [Serializable]
    struct Specific
    {
        [JsonProperty("name")] public string Name { get; private set; }
        [JsonProperty("dependsOfSize")] public SizeDepending sizeDepending { get; private set; }
    }
}
