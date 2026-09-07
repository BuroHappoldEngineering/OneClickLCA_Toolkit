using BH.oM.Base;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BH.Adapter.OneClickLCA.Objects
{
    public class CalculationResultRecord
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
        public virtual List<CalculationResultDataSetRecord> CalculationResultDataSets { get; set; }
    }
}
