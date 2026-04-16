using BH.oM.Base;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BH.oM.Adapters.OneClickLCA
{
    public class CalculationResult : BHoMObject
    {
        [JsonPropertyName("designId")]
        public string DesignId { get; set; }

        [JsonPropertyName("toolId")]
        public string ToolId { get; set; }

        [JsonPropertyName("calculationRuleId")]
        public string CalculationRuleId { get; set; }

        [JsonPropertyName("resultCategoryId")]
        public string ResultCategoryId { get; set; }

        [JsonPropertyName("result")]
        public double Result { get; set; }

        [JsonPropertyName("calculationResultDataSets")]
        public List<CalculationResultDataSet> CalculationResultDataSets { get; set; }
    }
}
