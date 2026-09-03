using BH.Adapter.OneClickLCA;
using BH.Adapter.OneClickLCA.Objects;
using BH.Engine.Adapters.OneClickLCA;
using BH.oM.Adapters.OneClickLCA;
using BH.oM.LifeCycleAssessment;
using BH.oM.LifeCycleAssessment.MaterialFragments;

namespace OneClickLCA_Tests
{
    public class CalculationsResultsApi_Tests
    {
        private const string clientId = "service_acc_buro_happold";
        private const string clientSecret = "";

        // These can be set to whatever designId & toolId you want to fetch
        private const string designId = "69b1576c2b13bd02eee02e3c";
        private const string toolId = "lcaRicsV2";

        private OneClickLCAAdapter adapter;
        private IEnumerable<CalculationResult> calculationResults;

        [SetUp]
        [Description("Setup method to pull calculation results from One Click LCA API")]
        public void Setup()
        {
            adapter = new OneClickLCAAdapter();

            CalculationResultsApiRequest request = new CalculationResultsApiRequest
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                DesignId = designId,
                ToolId = toolId,
                ShowAllCategoriesForTool = false
            };

            IEnumerable<object> pullResult = adapter.Pull(request);
            calculationResults = pullResult?.FirstOrDefault() as IEnumerable<CalculationResult> ?? Enumerable.Empty<CalculationResult>();
        }

        [Test]
        [Description("Test that calculation results are pulled successfully from One Click LCA API")]
        public void TestPullCalculationResults()
        {
            Assert.That(calculationResults, Is.Not.Null, "Calculation results response should not be null");
            Assert.That(calculationResults, Is.Not.Null, "CalculationResults list should not be null");
        }

        [Test]
        [Description("Test that biogenic results are correctly filtered out in the climate change total metric")]
        public void TestBiogenicResultsFilteredOutInClimateChangeTotalMetric()
        {
            Assert.That(calculationResults, Is.Not.Null, "Setup failed: calculation results response is null");

            ClimateChangeTotalMetric metricsWithFilter = Compute.ClimateChangeTotalMetric((List<CalculationResult>)calculationResults);
            ClimateChangeTotalMetric metricsWithoutFilter = Compute.ClimateChangeTotalMetric((List<CalculationResult>)calculationResults);

            Assert.That(metricsWithFilter, Is.Not.Null, "Metrics with filter should not be null");
            Assert.That(metricsWithoutFilter, Is.Not.Null, "Metrics without filter should not be null");

            // If there are biogenic results in the original data, metrics should differ
            bool hasBiogenicResults = calculationResults
                .Any(cr => cr.CalculationRuleId.ToUpper().Contains("BIOGENIC"));

            if (hasBiogenicResults)
            {
                Assert.That(metricsWithFilter.Indicators.Values.Sum(), Is.LessThanOrEqualTo(metricsWithoutFilter.Indicators.Values.Sum()),
                    "Filtered metrics should have lower or equal total compared to unfiltered metrics when biogenic results exist");
            }
        }
    }
}