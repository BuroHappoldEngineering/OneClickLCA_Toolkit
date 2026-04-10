using BH.oM.Adapters.OneClickLCA;
using BH.oM.Base.Attributes;
using BH.oM.LifeCycleAssessment.MaterialFragments;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BH.oM.LifeCycleAssessment;

namespace BH.Engine.Adapters.OneClickLCA
{
    public static partial class Compute
    {
        [Description("Maps calculation results to life cycle stage based on their calculation rule id.")]
        [Input("calculationResultsApiResponse", "API response from OneClickLCA")]
        [Input("filterOutBioGenicResults", "By default, biogenic results are filtered out, set parameter to false to include biogenic results")]
        [Output("metric", "Climate Change - Total, measured in kg CO2 eq (Carbon Dioxide equivalent, also referred to as embodied carbon), refers to the total of Biogenic, Land Use and Fossil resources which contribute to global warming. This environmental indicator forms part of an Environmental Product Declaration and should be evaluated based on the Quantity Type stated on the Environmental Product Declaration.")]
        public static ClimateChangeTotalMetric ToClimateChangeTotalMetric(CalculationResultsApiResponse calculationResultsApiResponse, bool filterOutBiogenicResults = false)
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

            ClimateChangeTotalMetric climateChangeTotalMetric = new ClimateChangeTotalMetric();


            // Collects all modules (life cycles stages) and order them by word length
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
                        if (!climateChangeTotalMetric.Indicators.ContainsKey(module))
                        {
                            climateChangeTotalMetric.Indicators.Add(module, 0);
                        }
                            
                        climateChangeTotalMetric.Indicators[module] += (calculationResult.Result);
                        break;
                    }
                }
            }

            return climateChangeTotalMetric;
        }


    }
}
