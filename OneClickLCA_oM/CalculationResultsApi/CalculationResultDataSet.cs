
using BH.oM.Base;

namespace BH.oM.Adapters.OneClickLCA
{
    public class CalculationResultDataSet: IObject
    {
        public virtual string DataSetId { get; set; }

        public virtual string ResourceId { get; set; }

        public virtual string ResourceName { get; set; }

        public virtual string ProfileId { get; set; }

        public virtual double Result { get; set; }

        public virtual string MainResourceId { get; set; }

        public virtual string MainResourceName { get; set; }

        public virtual string MainProfileId { get; set; }
    }
}
