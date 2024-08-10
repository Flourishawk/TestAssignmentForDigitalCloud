using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TestAssignmentForDigitalCloud.Models
{
    internal class Market
    {
        [JsonPropertyName("exchangeId")]
        public string ExchangeId { get; set; }

        [JsonPropertyName("baseId")]
        public string BaseId { get; set; }

        [JsonPropertyName("quoteId")]
        public string QuoteId { get; set; }

        [JsonPropertyName("quoteSymbol")]
        public string QuoteSymbol { get; set; }

        [JsonPropertyName("volumeUsd24Hr")]
        public string VolumeUsd24Hr { get; set; }

        [JsonPropertyName("priceUsd")]
        public string PriceUsd { get; set; }

        [JsonPropertyName("volumePercent")]
        public string VolumePercent { get; set; }
    }
}
