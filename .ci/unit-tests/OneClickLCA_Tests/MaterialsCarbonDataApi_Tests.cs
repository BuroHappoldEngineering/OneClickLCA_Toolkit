using BH.Engine.Adapters.OneClickLCA;
using BH.oM.Adapters.OneClickLCA;
using System.Text.Json;

namespace OneClickLCA_Tests
{
    public class MaterialsCarbonDataApi_Tests
    {
        private const string MinimalSearchJson =
            "{\"found\":1,\"hits\":[{\"document\":{\"_id\":\"testId\",\"nameEN\":\"Test material\",\"unitForData\":\"kg\"," +
            "\"impacts\":{\"A1-A3\":{\"impactGWP100_kgCO2e_total\":1.5}},\"epdNumber\":\"EPD-1\"},\"highlight\":{},\"highlights\":[]}]," +
            "\"page\":1,\"facet_counts\":[],\"request_params\":{\"collection_name\":\"c\",\"first_q\":\"*\",\"per_page\":10,\"q\":\"*\"}," +
            "\"search_cutoff\":false,\"search_time_ms\":1}";

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        [Test]
        public void MaterialsCarbonSearchJson_Deserializes_ToSearchResponse()
        {
            MaterialsCarbonDataSearchResponse? response = JsonSerializer.Deserialize<MaterialsCarbonDataSearchResponse>(MinimalSearchJson, JsonOptions);

            Assert.That(response, Is.Not.Null);
            Assert.That(response!.Found, Is.EqualTo(1));
            Assert.That(response.Hits, Is.Not.Null);
            Assert.That(response.Hits.Count, Is.EqualTo(1));
            Assert.That(response.Hits[0].Document, Is.Not.Null);
            Assert.That(response.Hits[0].Document!.NameEN, Is.EqualTo("Test material"));
            Assert.That(response.Hits[0].Document.Impacts, Is.Not.Null);
            Assert.That(response.Hits[0].Document.Impacts.ContainsKey("A1-A3"), Is.True);
        }

        [Test]
        public void ToEnvironmentalProductDeclarations_BuildsEpdFromSearchResponse()
        {
            MaterialsCarbonDataSearchResponse? response = JsonSerializer.Deserialize<MaterialsCarbonDataSearchResponse>(MinimalSearchJson, JsonOptions);
            Assert.That(response, Is.Not.Null);

            var epds = Compute.ToEnvironmentalProductDeclarations(response!);

            Assert.That(epds, Is.Not.Null);
            Assert.That(epds.Count, Is.EqualTo(1));
            Assert.That(epds[0].Name, Is.EqualTo("Test material"));
            Assert.That(epds[0].EnvironmentalMetrics, Is.Not.Null);
            Assert.That(epds[0].EnvironmentalMetrics.Count, Is.GreaterThan(0));
        }
    }
}
