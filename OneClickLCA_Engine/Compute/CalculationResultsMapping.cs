using BH.oM.Adapters.OneClickLCA;
using BH.oM.Base.Attributes;
using BH.oM.LifeCycleAssessment.MaterialFragments;
using BH.oM.LifeCycleAssessment;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;


namespace BH.Engine.Adapters.OneClickLCA
{
    public static partial class Compute
    {
        [Description("Maps calculation results objects to life cycle stage based on their calculation rule id.")]
        [Input("CalculationResultsApiResponse", "Description of the input. Will appear in the UI tooltip.")]
        [Input("FilterOutBiogenicResults", "By default, biogenic results are filtered out, set parameter to false to include biogenic results")]
        [Output("CalculationResults", "Results mapped based on their calculationRuleId")]
        public static CalculationResultsMapping CalculationResultsMapping(CalculationResultsApiResponse calculationResultsApiResponse, bool filterOutBiogenicResults = false)
        {
            if (calculationResultsApiResponse == null)
            {
                BH.Engine.Base.Compute.RecordError("Failed to collect data");
                return null;
            }

            List<CalculationResult> calculationResults;

            if (filterOutBiogenicResults)
            {
                calculationResults = FilterOutBiogenicCalculationResults(calculationResultsApiResponse);
            }
            else
            {
                calculationResults = calculationResultsApiResponse.CalculationResults;
            }

            var mappedData = new Dictionary<Module, List<CalculationResult>>();

            CalculationResultsMapping calculationResultsMapping = new CalculationResultsMapping();

            List<Module> modules = Enum.GetValues(typeof(Module)).Cast<Module>().ToList().OrderByDescending(s => s.ToString().Length).ToList();

            foreach (CalculationResult calculationResult in calculationResults)
            {
                // Null handling
                if (calculationResult == null || string.IsNullOrEmpty(calculationResult.ResultCategoryId))
                {
                    continue;
                }

                foreach (Module module in modules)
                {
                    string moduleToString = module.ToString().Replace("to", "-");

                    if (calculationResult.ResultCategoryId.StartsWith(moduleToString, StringComparison.OrdinalIgnoreCase))
                    {
                        if (!calculationResultsMapping.CalculationResults.ContainsKey(module))
                        {
                            calculationResultsMapping.CalculationResults.Add(module, new List<CalculationResult>());
                        }

                        calculationResultsMapping.CalculationResults[module].Add(calculationResult);
                        break;
                    }
                }
            }



            return calculationResultsMapping;
        }


    }
}
