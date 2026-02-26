using BH.Engine.Adapters.OneClickLCA.API;
using BH.oM.Base.Attributes;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BH.Engine.Adapters.OneClickLCA
{
    public static partial class Create
    {
        [Description("Get dictionary data (names of tools, categories and rules) for a design.")]
        [Input("clientId", "OAuth2 client id for token acquisition.")]
        [Input("clientSecret", "OAuth2 client secret for token acquisition.")]
        [Input("designId", "Design identifier (required).")]
        [Output("json, oclRequestId", "Raw JSON response from the API and the 'ocl-request-id' response header for tracing.")]
        public static async Task<(string json, string oclRequestId)> GetDictionaryDataAsync(string clientId, string clientSecret, string designId)
        {
            if (string.IsNullOrWhiteSpace(designId))
                throw new ArgumentException("designId is required", nameof(designId));

            var relative = $"calculation-results/dictionary?designId={Uri.EscapeDataString(designId)}";

            var token = await AcquireAccessTokenAsync(clientId, clientSecret).ConfigureAwait(false);
            var client = Client(token);
            var resp = await client.SendGetAsync(relative).ConfigureAwait(false);
            return (resp.Content, resp.OclRequestId);
        }

        // Synchronous wrapper for compatibility
        public static (string json, string oclRequestId) GetDictionaryData(string clientId, string clientSecret, string designId)
        {
            return GetDictionaryDataAsync(clientId, clientSecret, designId).GetAwaiter().GetResult();
        }

        // Overloads for bearer token
        [Description("Get dictionary data (names of tools, categories and rules) for a design using existing token.")]
        [Input("bearerToken", "Access token (Bearer) to use for the request.")]
        [Input("designId", "Design identifier (required).")]
        [Output("json, oclRequestId", "Raw JSON response from the API and the 'ocl-request-id' response header for tracing.")]
        public static async Task<(string json, string oclRequestId)> GetDictionaryDataAsync(string bearerToken, string designId)
        {
            if (string.IsNullOrWhiteSpace(bearerToken))
                throw new ArgumentException("bearerToken is required", nameof(bearerToken));
            if (string.IsNullOrWhiteSpace(designId))
                throw new ArgumentException("designId is required", nameof(designId));

            var relative = $"calculation-results/dictionary?designId={Uri.EscapeDataString(designId)}";

            var client = Client(bearerToken);
            var resp = await client.SendGetAsync(relative).ConfigureAwait(false);
            return (resp.Content, resp.OclRequestId);
        }

        // Synchronous wrapper
        public static (string json, string oclRequestId) GetDictionaryData(string bearerToken, string designId)
        {
            return GetDictionaryDataAsync(bearerToken, designId).GetAwaiter().GetResult();
        }
    }
}
