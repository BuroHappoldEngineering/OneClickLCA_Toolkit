using BH.oM.Adapters.OneClickLCA;
using BH.oM.Base;
using BH.oM.Base.Attributes;
using BH.oM.Base.Attributes.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BH.Engine.Adapters.OneClickLCA
{
    public static partial class Compute
    {
        public static Dictionary<string, List<CalculationResult>> GroupCalculationResultsByCalculationRuleId(CalculationResultsAndDictionaryDataApiResponse response, string toolId)
        {

            Dictionary<string, DesignToolDictionaryMapping> mappedDictionaryData = MapDictionaryDataByToolId(response.DictionaryDataApiResponse.DictionaryData);

            DesignToolDictionaryMapping designToolDictionaryMapping = mappedDictionaryData[toolId];

            Dictionary<string, string> rules = designToolDictionaryMapping.Rules;
            Dictionary <string, List<CalculationResult>> calculationResultsMapping = new Dictionary<string, List<CalculationResult>>();

            foreach (CalculationResult calculationResult in response.CalculationResultsApiResponse.CalculationResults)
            {
                string rule = calculationResult.CalculationRuleId;

                if (designToolDictionaryMapping.Rules.ContainsKey(rule))
                {
                    calculationResultsMapping[rule].Add(calculationResult);
                }
                
            }

            return calculationResultsMapping;
        }
    }
}
