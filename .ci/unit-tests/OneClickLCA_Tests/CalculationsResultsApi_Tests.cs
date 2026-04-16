using BH.Adapter.OneClickLCA;
using BH.Engine.Adapters.OneClickLCA;
using BH.oM.Adapters.OneClickLCA;
using BH.oM.LifeCycleAssessment;
using BH.oM.LifeCycleAssessment.MaterialFragments;

namespace OneClickLCA_Tests
{
    public class CalculationsResultsApi_Tests
    {
        private const string clientId = "service_acc_buro_happold";
        private const string clientSecret = "3p0vfSnj1tjvZsZEjHICkYUhVj6GYCAZ";

        // These can be set to whatever designId & toolId you want to fetch
        private const string designId = "69b1576c2b13bd02eee02e3c";
        private const string toolId = "lcaRicsV2";

        private OneClickLCAAdapter adapter;
        private CalculationResultsApiResponse calculationResultsApiResponse;

        [SetUp]
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
            calculationResultsApiResponse = pullResult?.FirstOrDefault() as CalculationResultsApiResponse;
        }

        [Test]
        public void TestPullCalculationResults()
        {
            Assert.That(calculationResultsApiResponse, Is.Not.Null, "Calculation results response should not be null");
            Assert.That(calculationResultsApiResponse.CalculationResults, Is.Not.Null, "CalculationResults list should not be null");
        }

        [Test]
        public void TestBiogenicResultsFilteredOutInClimateChangeTotalMetric()
        {
            Assert.That(calculationResultsApiResponse, Is.Not.Null, "Setup failed: calculation results response is null");

            ClimateChangeTotalMetric metricsWithFilter = Compute.ToClimateChangeTotalMetric(calculationResultsApiResponse, filterOutBiogenicResults: true);
            ClimateChangeTotalMetric metricsWithoutFilter = Compute.ToClimateChangeTotalMetric(calculationResultsApiResponse, filterOutBiogenicResults: false);

            Assert.That(metricsWithFilter, Is.Not.Null, "Metrics with filter should not be null");
            Assert.That(metricsWithoutFilter, Is.Not.Null, "Metrics without filter should not be null");

            // If there are biogenic results in the original data, metrics should differ
            bool hasBiogenicResults = calculationResultsApiResponse.CalculationResults
                .Any(cr => cr.CalculationRuleId.ToUpper().Contains("BIOGENIC"));

            if (hasBiogenicResults)
            {
                Assert.That(metricsWithFilter.Indicators.Values.Sum(), Is.LessThanOrEqualTo(metricsWithoutFilter.Indicators.Values.Sum()),
                    "Filtered metrics should have lower or equal total compared to unfiltered metrics when biogenic results exist");
            }
        }
    }
}