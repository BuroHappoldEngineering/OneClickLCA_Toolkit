using BH.oM.Base;
using BH.oM.Base.Attributes;
using BH.oM.Data.Requests;
using System.ComponentModel;

namespace BH.oM.Adapters.OneClickLCA
{
    [Description("Request dictionary data and calculation results for a design and tool in one Pull (GET /calculation-results/dictionary and GET /calculation-results).")]
    public class CalculationResultsAndDictionaryDataApiRequest : BHoMObject, IRequest
    {
        [Description("OAuth2 client identifier used for client credentials authentication.")]
        public virtual string ClientId { get; set; } = "";

        [Description("OAuth2 client secret used for client credentials authentication.")]
        public virtual string ClientSecret { get; set; } = "";

        [Description("Unique identifier for the design (24-character alphanumeric).")]
        public virtual string DesignId { get; set; } = "";

        [Description("Identifier for the tool used in the calculation (e.g. flexibleEPDTool, simplifiedLifeCycleCarbon).")]
        public virtual string ToolId { get; set; } = "";

        [Description("When true, include all result categories for the tool including dummy categories not shown on the result page. Defaults to false.")]
        public virtual bool ShowAllCategoriesForTool { get; set; } = false;
    }
}
