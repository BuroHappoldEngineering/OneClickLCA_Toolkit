using System.Text.Json.Serialization;

namespace BH.oM.Adapters.OneClickLCA
{
    public class CalculationTotalResult
    {
        [JsonPropertyName("calculationRuleId")]
        public virtual string CalculationRuleId { get; set; }

        [JsonPropertyName("result")]
        public virtual double Result { get; set; }

        [JsonPropertyName("resultUnit")]
        public virtual string ResultUnit { get; set; }
    }
}
