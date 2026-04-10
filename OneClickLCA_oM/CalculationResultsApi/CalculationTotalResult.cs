using System.Text.Json.Serialization;

namespace BH.oM.Adapters.OneClickLCA
{
    public class CalculationTotalResult
    {
        [JsonPropertyName("calculationRuleId")]
        public string CalculationRuleId { get; set; }

        [JsonPropertyName("result")]
        public double Result { get; set; }

        [JsonPropertyName("resultUnit")]
        public string ResultUnit { get; set; }
    }
}
