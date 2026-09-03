using BH.oM.Base;
using System.Collections.Generic;

namespace BH.oM.Adapters.OneClickLCA
{
    public class CalculationResult : BHoMObject
    {
        public virtual string DesignId { get; set; }

        public virtual string ToolId { get; set; }

        public virtual string CalculationRuleId { get; set; }

        public virtual string ResultCategoryId { get; set; }

        public virtual double Result { get; set; }

        public virtual List<CalculationResultDataSet> CalculationResultDataSets { get; set; }
    }
}
