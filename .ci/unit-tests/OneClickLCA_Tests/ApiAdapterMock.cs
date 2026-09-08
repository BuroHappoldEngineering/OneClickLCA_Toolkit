using BH.Adapter;
using BH.oM.Adapter;
using BH.oM.Adapters.HTTP;
using BH.oM.Data.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneClickLCA_Tests
{
    internal class ApiAdapterMock: BHoMAdapter, IBHoMAdapter
    {
        public override IEnumerable<object> Pull(IRequest request, PullType pullType = PullType.AdapterDefault, ActionConfig actionConfig = null)
        {
            GetRequest getRequest = (GetRequest)request;

            if (getRequest.BaseUrl.Contains("materials-carbon-data"))
            {
                return new List<object>() {
            "{\"found\":1,\"hits\":[{\"document\":{\"_id\":\"testId\",\"nameEN\":\"Test material\",\"unitForData\":\"kg\"," +
            "\"impacts\":{\"A1-A3\":{\"impactGWP100_kgCO2e_total\":1.5}},\"epdNumber\":\"EPD-1\"},\"highlight\":{},\"highlights\":[]}]," +
            "\"page\":1,\"facet_counts\":[],\"request_params\":{\"collection_name\":\"c\",\"first_q\":\"*\",\"per_page\":10,\"q\":\"*\"}," +
            "\"search_cutoff\":false,\"search_time_ms\":1}" };
            }
            else if (getRequest.BaseUrl.Contains("projects"))
            {
                return new List<object>() { "expectedProjectsResponse" };
            }
            else if (getRequest.BaseUrl.Contains("calculation-results"))
            {
                if (getRequest.BaseUrl.Contains("dictionary"))
                {
                    return new List<object>() { "expectedDictionaryResponse" };
                }
                return new List<object>() { "expectedCalculationResultsResponse" };
            }

            throw new NotImplementedException($"The requested uri was not expected: {getRequest.BaseUrl}");
        }
    }
}