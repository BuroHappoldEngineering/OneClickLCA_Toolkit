using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BH.Engine.Adapters.OneClickLCA.API
{
    public class ApiResponse
    {
        public string Content { get; set; }
        public string OclRequestId { get; set; }
        public int StatusCode { get; set; }
    }

    public class OneClickLCAClient : IDisposable
    {
        private static readonly Uri DefaultBaseUri = new Uri("https://oneclicklcaapp.com/results-api/");
        private readonly HttpClient _http;
        private bool _disposed;

        public OneClickLCAClient(string bearerToken, Uri baseUri = null, HttpMessageHandler handler = null)
        {
            if (baseUri == null)
                baseUri = DefaultBaseUri;

            _http = handler != null ? new HttpClient(handler) : new HttpClient();
            _http.BaseAddress = baseUri;
            SetBearerToken(bearerToken);
            _http.DefaultRequestHeaders.Accept.Clear();
            _http.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void SetBearerToken(string bearerToken)
        {
            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                _http.DefaultRequestHeaders.Authorization = null;
            }
            else
            {
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
            }
        }

        /// <summary>
        /// Send a GET request to the provided relative URI and return ApiResponse. Synchronous wrapper.
        /// </summary>
        public ApiResponse SendGet(string relativeUri)
        {
            return SendGetAsync(relativeUri).GetAwaiter().GetResult();
        }

        public async Task<ApiResponse> SendGetAsync(string relativeUri)
        {
            using (var resp = await _http.GetAsync(relativeUri).ConfigureAwait(false))
            {
                var content = resp.Content == null ? string.Empty : await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                resp.Headers.TryGetValues("ocl-request-id", out var idValues);
                string requestId = idValues != null ? string.Join(",", idValues) : null;

                var apiResp = new ApiResponse
                {
                    Content = content,
                    OclRequestId = requestId,
                    StatusCode = (int)resp.StatusCode
                };

                if (!resp.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Request to '{relativeUri}' failed with {(int)resp.StatusCode} - {resp.ReasonPhrase}. Response content: {content}");
                }

                return apiResp;
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _http?.Dispose();
                _disposed = true;
            }
        }
    }
}