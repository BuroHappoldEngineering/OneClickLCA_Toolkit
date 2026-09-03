using BH.oM.Adapters.OneClickLCA;
using BH.oM.Base.Attributes;
using BH.oM.LifeCycleAssessment.MaterialFragments;
using BH.oM.LifeCycleAssessment;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;


namespace BH.Engine.Adapters.OneClickLCA
{
    public static partial class Query
    {
        [Description("Filter a list of calculation results to remove biogenic results")]
        [Input("calculationResults", "The list of calculation results.")]
        [Output("filteredResults", "The list of calculation results with biogenic results removed.")]
        public static IEnumerable<CalculationResult> FilterNonBiogenicCalculationResults (this IEnumerable<CalculationResult> calculationResults)
        {
            return calculationResults.Where(l => !l.CalculationRuleId.ToUpper().Contains("BIOGENIC"));
        }
    }
}
