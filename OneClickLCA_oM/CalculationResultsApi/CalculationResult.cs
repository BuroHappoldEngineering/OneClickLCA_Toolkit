using BH.oM.Base;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BH.oM.Adapters.OneClickLCA
{
    public class CalculationResult : BHoMObject
    {
        [JsonPropertyName("designId")]
        public virtual string DesignId { get; set; }

        [JsonPropertyName("toolId")]
        public virtual string ToolId { get; set; }

        [JsonPropertyName("calculationRuleId")]
        public virtual string CalculationRuleId { get; set; }

        [JsonPropertyName("resultCategoryId")]
        public virtual string ResultCategoryId { get; set; }

        [JsonPropertyName("result")]
        public virtual double Result { get; set; }

        [JsonPropertyName("calculationResultDataSets")]
        public virtual List<CalculationResultDataSet> CalculationResultDataSets { get; set; }
    }
}
