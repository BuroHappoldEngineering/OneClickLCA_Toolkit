using BH.Engine.Adapters.OneClickLCA;
using BH.oM.Adapters.OneClickLCA;

namespace OneClickLCA_Tests
{
    public class DictionaryDataMapping_Tests
    {
        [Test]
        public void MapDictionaryDataByToolId_ProducesOneMappingPerToolWithRulesAndCategories()
        {
            DictionaryData data = new DictionaryData
            {
                DesignId = "design-1",
                ToolData = new List<ToolData>
                {
                    new ToolData
                    {
                        Tool = new Dictionary<string, string> { { "lcaRicsV2", "RICS tool name" } },
                        Rules = new Dictionary<string, string> { { "GWP-RICS_perM2", "GWP per m2" } },
                        Categories = new Dictionary<string, string> { { "A1-A3-ProductStage", "Product stage" } }
                    }
                }
            };

            Dictionary<string, DesignToolDictionaryMapping> mappings = Compute.MapDictionaryDataByToolId(data);

            Assert.That(mappings, Is.Not.Null);
            Assert.That(mappings.Count, Is.EqualTo(1));
            Assert.That(mappings.ContainsKey("lcaRicsV2"), Is.True);
            DesignToolDictionaryMapping row = mappings["lcaRicsV2"];
            Assert.That(row.DesignId, Is.EqualTo("design-1"));
            Assert.That(row.ToolId, Is.EqualTo("lcaRicsV2"));
            Assert.That(row.ToolDisplayName, Is.EqualTo("RICS tool name"));
            Assert.That(row.Rules["GWP-RICS_perM2"], Is.EqualTo("GWP per m2"));
            Assert.That(row.Categories["A1-A3-ProductStage"], Is.EqualTo("Product stage"));
        }

        [Test]
        public void MapDictionaryDataByToolId_NullInput_ReturnsEmptyDictionary()
        {
            Dictionary<string, DesignToolDictionaryMapping> mappings = Compute.MapDictionaryDataByToolId(null!);
            Assert.That(mappings, Is.Not.Null);
            Assert.That(mappings.Count, Is.EqualTo(0));
        }
    }
}
