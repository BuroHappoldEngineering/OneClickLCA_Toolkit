using System.Text.Json.Serialization;

namespace BH.oM.Adapters.OneClickLCA
{
    public class CalculationResultDataSet
    {
        [JsonPropertyName("dataSetId")]
        public virtual string DataSetId { get; set; }

        [JsonPropertyName("resourceId")]
        public virtual string ResourceId { get; set; }

        [JsonPropertyName("resourceName")]
        public virtual string ResourceName { get; set; }

        [JsonPropertyName("profileId")]
        public virtual string ProfileId { get; set; }

        [JsonPropertyName("result")]
        public virtual double Result { get; set; }

        [JsonPropertyName("mainResourceId")]
        public virtual string MainResourceId { get; set; }

        [JsonPropertyName("mainResourceName")]
        public virtual string MainResourceName { get; set; }

        [JsonPropertyName("mainProfileId")]
        public virtual string MainProfileId { get; set; }
    }
}
