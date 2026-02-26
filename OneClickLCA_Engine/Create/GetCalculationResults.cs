using BH.Engine.Adapters.OneClickLCA.API;
using BH.oM.Base.Attributes;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BH.Engine.Adapters.OneClickLCA
{
    public static partial class Create
    {
        [Description("Get calculation results for a given designId and toolId.")]
        [Input("clientId", "OAuth2 client id for token acquisition.")]
        [Input("clientSecret", "OAuth2 client secret for token acquisition.")]
        [Input("designId", "Design identifier (24 chars).")]
        [Input("toolId", "Tool identifier used in the calculation.")]
        [Input("showAllCategoriesForTool", "Optional: show all categories for the tool (default false).")]
        [Output("json, oclRequestId", "Raw JSON response from the API and the 'ocl-request-id' response header for tracing.")]
        public static async Task<(string json, string oclRequestId)> GetCalculationResultsAsync(string clientId, string clientSecret, string designId, string toolId, bool showAllCategoriesForTool = false)
        {
            if (string.IsNullOrWhiteSpace(designId))
                throw new ArgumentException("designId is required", nameof(designId));
            if (string.IsNullOrWhiteSpace(toolId))
                throw new ArgumentException("toolId is required", nameof(toolId));

            var q = new System.Collections.Generic.List<string> { $"designId={Uri.EscapeDataString(designId)}", $"toolId={Uri.EscapeDataString(toolId)}" };
            if (showAllCategoriesForTool)
                q.Add($"showAllCategoriesForTool={showAllCategoriesForTool.ToString().ToLowerInvariant()}");

            var relative = "calculation-results" + (q.Count > 0 ? "?" + string.Join("&", q) : string.Empty);

            var token = await AcquireAccessTokenAsync(clientId, clientSecret).ConfigureAwait(false);
            var client = Client(token);
            var resp = await client.SendGetAsync(relative).ConfigureAwait(false);
            return (resp.Content, resp.OclRequestId);
        }

        // Synchronous wrapper for compatibility
        public static (string json, string oclRequestId) GetCalculationResults(string clientId, string clientSecret, string designId, string toolId, bool showAllCategoriesForTool = false)
        {
            return GetCalculationResultsAsync(clientId, clientSecret, designId, toolId, showAllCategoriesForTool).GetAwaiter().GetResult();
        }

        // New overloads that accept an externally-provided bearer token
        [Description("Get calculation results for a given designId and toolId using an existing access token.")]
        [Input("bearerToken", "Access token (Bearer) to use for the request.")]
        [Input("designId", "Design identifier (24 chars).")]
        [Input("toolId", "Tool identifier used in the calculation.")]
        [Input("showAllCategoriesForTool", "Optional: show all categories for the tool (default false).")]
        [Output("json, oclRequestId", "Raw JSON response from the API and the 'ocl-request-id' response header for tracing.")]
        public static async Task<(string json, string oclRequestId)> GetCalculationResultsAsync(string bearerToken, string designId, string toolId, bool showAllCategoriesForTool = false)
        {
            if (string.IsNullOrWhiteSpace(bearerToken))
                throw new ArgumentException("bearerToken is required", nameof(bearerToken));
            if (string.IsNullOrWhiteSpace(designId))
                throw new ArgumentException("designId is required", nameof(designId));
            if (string.IsNullOrWhiteSpace(toolId))
                throw new ArgumentException("toolId is required", nameof(toolId));

            var q = new System.Collections.Generic.List<string> { $"designId={Uri.EscapeDataString(designId)}", $"toolId={Uri.EscapeDataString(toolId)}" };
            if (showAllCategoriesForTool)
                q.Add($"showAllCategoriesForTool={showAllCategoriesForTool.ToString().ToLowerInvariant()}");

            var relative = "calculation-results" + (q.Count > 0 ? "?" + string.Join("&", q) : string.Empty);

            var client = Client(bearerToken);
            var resp = await client.SendGetAsync(relative).ConfigureAwait(false);
            return (resp.Content, resp.OclRequestId);
        }

        // Synchronous wrapper
        public static (string json, string oclRequestId) GetCalculationResults(string bearerToken, string designId, string toolId, bool showAllCategoriesForTool = false)
        {
            return GetCalculationResultsAsync(bearerToken, designId, toolId, showAllCategoriesForTool).GetAwaiter().GetResult();
        }
    }
}