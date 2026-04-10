using System.Text.Json.Serialization;

namespace BH.oM.Adapters.OneClickLCA
{
    public class CalculationResultDataSet
    {
        [JsonPropertyName("dataSetId")]
        public string DataSetId { get; set; }

        [JsonPropertyName("resourceId")]
        public string ResourceId { get; set; }

        [JsonPropertyName("resourceName")]
        public string ResourceName { get; set; }

        [JsonPropertyName("profileId")]
        public string ProfileId { get; set; }

        [JsonPropertyName("result")]
        public double Result { get; set; }

        [JsonPropertyName("mainResourceId")]
        public string MainResourceId { get; set; }

        [JsonPropertyName("mainResourceName")]
        public string MainResourceName { get; set; }

        [JsonPropertyName("mainProfileId")]
        public string MainProfileId { get; set; }
    }
}
