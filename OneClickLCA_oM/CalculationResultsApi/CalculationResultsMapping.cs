using BH.oM.Base;
using BH.oM.Base.Attributes;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using BH.oM.LifeCycleAssessment;

namespace BH.oM.Adapters.OneClickLCA
{
    public class CalculationResultsMapping : BHoMObject, IDynamicObject
    {
        [DynamicProperty]
        [Description("Calculation results mapped to life cycle stage")]
        public virtual Dictionary<Module, List<CalculationResult>> CalculationResults { get; set; } = new Dictionary<Module, List<CalculationResult>>();
    }
}
