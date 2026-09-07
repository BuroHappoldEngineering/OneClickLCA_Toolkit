using BH.Adapter;
using BH.oM.Adapter;
using BH.oM.Adapters.SQL;
using BH.oM.Base;
using BH.oM.Data.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneClickLCA_Tests
{
    /// <summary>
    /// This is a mock BHoMAdapter used for testing that the main adapter flow is working correctly,
    /// it does not return an actually valid API key, nor does it need to.
    /// <br />
    /// This adapter is solely for testing.
    /// </summary>
    internal class AuthAdapterMock: BHoMAdapter, IBHoMAdapter
    {
        /// <summary>
        /// mock pull method that directly returns a "valid API key"
        /// </summary>
        public override IEnumerable<object> Pull(IRequest request, PullType pullType = PullType.AdapterDefault, ActionConfig actionConfig = null)
        {
            TableRequest tRequest = request as TableRequest;

            if (tRequest == null)
            {
                return null;
            }

            CustomObject rtn = new CustomObject();

            rtn.CustomData["ClientId"] = "ValidClientId";
            rtn.CustomData["ClientSecret"] = "ValidAPIKey";

            return new List<object>() { rtn };
        }
    }
}
