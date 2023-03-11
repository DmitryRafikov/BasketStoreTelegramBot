using Newtonsoft.Json;

namespace BasketStoreTelegramBot.Features.ProductInformation.Structure
{
    struct SizeDepending
    {
        [JsonProperty("type")] public string Type;
        [JsonProperty("min")] public int Min;
        [JsonProperty("max")] public int Max;
    }
}
