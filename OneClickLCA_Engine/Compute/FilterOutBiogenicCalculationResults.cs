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
        [Description("Description of the method. Will appear in the UI tooltip.")]
        [Input("someInput1", "Description of the input. Will appear in the UI tooltip.")]
        [Input("someInput2", "Description of the input. Will appear in the UI tooltip.")]
        [Output("outputName", "Description of the output. Will appear in the UI tooltip.")]
        public static List<CalculationResult> FilterOutBiogenicCalculationResults(CalculationResultsApiResponse response)
        {
            return response.CalculationResults.Where(l => !l.CalculationRuleId.ToUpper().Contains("BIOGENIC")).ToList();
        }
    }
}
