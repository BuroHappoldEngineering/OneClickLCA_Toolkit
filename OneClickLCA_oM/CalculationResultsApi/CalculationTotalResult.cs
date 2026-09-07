using BH.oM.Base;

namespace BH.oM.Adapters.OneClickLCA
{
    public class CalculationTotalResult : BHoMObject
    {
        public virtual string CalculationRuleId { get; set; }

        public virtual double Result { get; set; }

        public virtual string ResultUnit { get; set; }
    }
}
