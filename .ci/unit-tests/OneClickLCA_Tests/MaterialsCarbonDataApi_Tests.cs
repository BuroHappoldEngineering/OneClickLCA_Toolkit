using BH.Engine.Adapters.OneClickLCA;
using BH.oM.Adapters.OneClickLCA;
using System.Text.Json;
using BH.Adapter.OneClickLCA;
using BH.oM.LifeCycleAssessment.MaterialFragments;
using BH.oM.LifeCycleAssessment;
using BH.oM.LifeCycleAssessment.Fragments;

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
        [Description("Test that a materials-carbon document extracted from a search response can be converted to an EPD using the public converter.")]
        public void MaterialsCarbonDocumentJson_Converts_ToEpd()
        {
            using JsonDocument doc = JsonDocument.Parse(MinimalSearchJson);
            JsonElement documentElement = doc.RootElement.GetProperty("hits")[0].GetProperty("document");

            string documentJson = documentElement.GetRawText();

            var epd = BH.Adapter.OneClickLCA.Convert.ToEnvironmentalProductDeclaration(documentJson);

            Assert.That(epd, Is.Not.Null);
            Assert.That(epd!.Name, Is.EqualTo("Test material"));
            Assert.That(epd.EnvironmentalMetrics, Is.Not.Null);
            Assert.That(epd.EnvironmentalMetrics.Count, Is.GreaterThan(0));
        }

        [Description("Test that the public converter returns null for invalid input and produces expected EPD for valid document JSON.")]
        [Test]
        public void ToEnvironmentalProductDeclarationPublicConverterBehaviour()
        {
            // Null or empty input should return null
            var nullResult = BH.Adapter.OneClickLCA.Convert.ToEnvironmentalProductDeclaration((string)null);
            Assert.That(nullResult, Is.Null);

            var emptyResult = BH.Adapter.OneClickLCA.Convert.ToEnvironmentalProductDeclaration(string.Empty);
            Assert.That(emptyResult, Is.Null);

            // Valid document JSON produces an EPD
            using JsonDocument doc = JsonDocument.Parse(MinimalSearchJson);
            JsonElement documentElement = doc.RootElement.GetProperty("hits")[0].GetProperty("document");
            string documentJson = documentElement.GetRawText();

            var epd = BH.Adapter.OneClickLCA.Convert.ToEnvironmentalProductDeclaration(documentJson);
            Assert.That(epd, Is.Not.Null);
            Assert.That(epd!.Name, Is.EqualTo("Test material"));
            Assert.That(epd.EnvironmentalMetrics, Is.Not.Null);
            Assert.That(epd.EnvironmentalMetrics.Count, Is.GreaterThan(0));
        }

        [Test]
        [Description("Test that the Convert.ToEnvironmentalProductDeclaration method correctly builds an EnvironmentalProductDeclaration from a document JSON.")]
        public void ConvertToEnvironmentalProductDeclaration_BuildsEpdFromDocumentJson()
        {
            var doc = JsonDocument.Parse(MinimalSearchJson);
            var documentElement = doc.RootElement.GetProperty("hits")[0].GetProperty("document");
            string documentJson = documentElement.GetRawText();

            var epd = BH.Adapter.OneClickLCA.Convert.ToEnvironmentalProductDeclaration(documentJson);

            Assert.That(epd, Is.Not.Null);
            Assert.That(epd.Name, Is.EqualTo("Test material"));
            Assert.That(epd.EnvironmentalMetrics, Is.Not.Null);
        }
    }
}
