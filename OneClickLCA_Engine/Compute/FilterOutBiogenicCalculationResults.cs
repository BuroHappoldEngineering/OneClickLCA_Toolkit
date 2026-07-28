using BH.oM.Adapters.OneClickLCA;
using BH.oM.Base.Attributes;
using BH.oM.LifeCycleAssessment.MaterialFragments;
using BH.oM.LifeCycleAssessment;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;


namespace BH.Engine.Adapters.OneClickLCA
{
    public static partial class Compute
    {
        [Description("Filters out biogenic calculation results from the API response.")]
        [Input("response", "The API response containing calculation results.")]
        [Output("filteredResults", "The list of calculation results with biogenic results removed.")]
        public static List<CalculationResult> FilterOutBiogenicCalculationResults(CalculationResultsApiResponse response)
        {
            return response.CalculationResults.Where(l => !l.CalculationRuleId.ToUpper().Contains("BIOGENIC")).ToList();
        }
    }
}
