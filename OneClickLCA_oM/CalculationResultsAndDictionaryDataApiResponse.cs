using BH.oM.Base;
using BH.oM.Base.Attributes;
using System.ComponentModel;

namespace BH.oM.Adapters.OneClickLCA
{
    [Description("Combined dictionary and calculation results returned from CalculationResultsAndDictionaryDataApiRequest.")]
    public class CalculationResultsAndDictionaryDataApiResponse : BHoMObject
    {
        [Description("Tool, category, and rule names from GET calculation-results/dictionary.")]
        public virtual DictionaryDataApiResponse DictionaryDataApiResponse { get; set; }

        [Description("Calculation results from GET calculation-results.")]
        public virtual CalculationResultsApiResponse CalculationResultsApiResponse { get; set; }
    }
}


