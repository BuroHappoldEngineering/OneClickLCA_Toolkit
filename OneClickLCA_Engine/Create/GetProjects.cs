using BH.Engine.Adapters.OneClickLCA.API;
using BH.oM.Base.Attributes;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BH.Engine.Adapters.OneClickLCA
{
    public static partial class Create
    {
        [Description("Get list of Projects with associated designs from One Click LCA Results API.")]
        [Input("clientId", "OAuth2 client id for token acquisition.")]
        [Input("clientSecret", "OAuth2 client secret for token acquisition.")]
        [Input("page", "Page number (defaults to 1).")]
        [Input("limit", "Number of records per page (defaults to 20).")]
        [Input("lastUpdatedAfter", "Optional date filter (yyyy-MM-dd) to return project-level info updated after this date.")]
        [Output("json, oclRequestId", "Raw JSON response from the API and the 'ocl-request-id' response header for tracing.")]
        public static async Task<(string json, string oclRequestId)> GetProjectsAsync(string clientId, string clientSecret, int page = 1, int limit = 20, string lastUpdatedAfter = null)
        {
            if (page < 1)
                page = 1;
            if (limit < 1)
                limit = 20;

            var q = new System.Collections.Generic.List<string> { $"page={Uri.EscapeDataString(page.ToString())}", $"limit={Uri.EscapeDataString(limit.ToString())}" };
            if (!string.IsNullOrWhiteSpace(lastUpdatedAfter))
                q.Add($"lastUpdatedAfter={Uri.EscapeDataString(lastUpdatedAfter)}");

            var relative = "projects" + (q.Count > 0 ? "?" + string.Join("&", q) : string.Empty);

            var token = await AcquireAccessTokenAsync(clientId, clientSecret).ConfigureAwait(false);
            var client = Client(token);
            var resp = await client.SendGetAsync(relative).ConfigureAwait(false);
            return (resp.Content, resp.OclRequestId);
        }

        // Synchronous wrapper for compatibility
        public static (string json, string oclRequestId) GetProjects(string clientId, string clientSecret, int page = 1, int limit = 20, string lastUpdatedAfter = null)
        {
            return GetProjectsAsync(clientId, clientSecret, page, limit, lastUpdatedAfter).GetAwaiter().GetResult();
        }

        // Overloads accepting bearer token
        [Description("Get list of Projects with associated designs using existing access token.")]
        [Input("bearerToken", "Access token (Bearer) to use for the request.")]
        [Input("page", "Page number (defaults to 1).")]
        [Input("limit", "Number of records per page (defaults to 20).")]
        [Input("lastUpdatedAfter", "Optional date filter (yyyy-MM-dd) to return project-level info updated after this date.")]
        [Output("json, oclRequestId", "Raw JSON response from the API and the 'ocl-request-id' response header for tracing.")]
        public static async Task<(string json, string oclRequestId)> GetProjectsAsync(string bearerToken, int page = 1, int limit = 20, string lastUpdatedAfter = null)
        {
            if (string.IsNullOrWhiteSpace(bearerToken))
                throw new ArgumentException("bearerToken is required", nameof(bearerToken));

            if (page < 1)
                page = 1;
            if (limit < 1)
                limit = 20;

            var q = new System.Collections.Generic.List<string> { $"page={Uri.EscapeDataString(page.ToString())}", $"limit={Uri.EscapeDataString(limit.ToString())}" };
            if (!string.IsNullOrWhiteSpace(lastUpdatedAfter))
                q.Add($"lastUpdatedAfter={Uri.EscapeDataString(lastUpdatedAfter)}");

            var relative = "projects" + (q.Count > 0 ? "?" + string.Join("&", q) : string.Empty);

            var client = Client(bearerToken);
            var resp = await client.SendGetAsync(relative).ConfigureAwait(false);
            return (resp.Content, resp.OclRequestId);
        }

        // Synchronous wrapper
        public static (string json, string oclRequestId) GetProjects(string bearerToken, int page = 1, int limit = 20, string lastUpdatedAfter = null)
        {
            return GetProjectsAsync(bearerToken, page, limit, lastUpdatedAfter).GetAwaiter().GetResult();
        }
    }
}