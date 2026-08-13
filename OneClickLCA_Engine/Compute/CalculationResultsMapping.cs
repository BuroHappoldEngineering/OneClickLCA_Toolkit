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
        [Input("calculationResultsApiResponse", "The API response containing calculation results.")]
        [Output("CalculationResults", "Results mapped based on their calculationRuleId.")]
        public static CalculationResultsMapping CalculationResultsMapping(List<CalculationResult> calculationResults)
        {
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
