using BH.Engine.Adapters.OneClickLCA.API;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BH.Engine.Adapters.OneClickLCA
{
    public static partial class Create
    {
        private static readonly object s_clientLock = new object();
        private static OneClickLCAClient s_client;

        // Token cache fields
        private static readonly SemaphoreSlim s_tokenSemaphore = new SemaphoreSlim(1, 1);
        private static string s_token;
        private static DateTime s_tokenExpiryUtc = DateTime.MinValue;
        // Refresh the token this many seconds before actual expiry to avoid races
        private static readonly TimeSpan s_tokenRefreshBefore = TimeSpan.FromSeconds(60);

        /// <summary>
        /// Create or update the singleton client with a bearer token.
        /// Use this when you already have an access token.
        /// </summary>
        public static OneClickLCAClient Client(string bearerToken)
        {
            lock (s_clientLock)
            {
                if (s_client == null)
                    s_client = new OneClickLCAClient(bearerToken);
                else
                    s_client.SetBearerToken(bearerToken);

                return s_client;
            }
        }

        /// <summary>
        /// Validate an externally-provided access token by making a lightweight API call and, if valid, set
        /// the singleton client to use this token. Returns true if token is valid and set.
        /// </summary>
        public static async Task<bool> ValidateAndSetAccessTokenAsync(string bearerToken)
        {
            if (string.IsNullOrWhiteSpace(bearerToken))
                throw new ArgumentException("bearerToken is required", nameof(bearerToken));

            // Use a temporary client to validate the token by calling a lightweight endpoint
            using (var temp = new OneClickLCAClient(bearerToken))
            {
                try
                {
                    // Request a single page with limit 1 — minimal payload
                    var resp = await temp.SendGetAsync("projects?page=1&limit=1").ConfigureAwait(false);
                    // If the request succeeded, set the singleton client to use this token
                    Client(bearerToken);
                    // Also update token cache to reflect externally provided token; set expiry to a short interval
                    lock (s_tokenSemaphore)
                    {
                        s_token = bearerToken;
                        // set expiry to near future to force refresh if AcquireAccessToken is used
                        s_tokenExpiryUtc = DateTime.UtcNow.AddMinutes(5);
                    }

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Synchronous wrapper for ValidateAndSetAccessTokenAsync.
        /// </summary>
        public static bool ValidateAndSetAccessToken(string bearerToken)
        {
            return ValidateAndSetAccessTokenAsync(bearerToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Create or update the singleton client using client credentials.
        /// This will acquire an access token internally and will respect cached token expiry.
        /// </summary>
        public static OneClickLCAClient Client(string clientId, string clientSecret)
        {
            var token = AcquireAccessToken(clientId, clientSecret);
            return Client(token);
        }

        /// <summary>
        /// Acquire an access token using OAuth2 client credentials with caching. Async version.
        /// </summary>
        public static async Task<string> AcquireAccessTokenAsync(string clientId, string clientSecret)
        {
            if (string.IsNullOrWhiteSpace(clientId))
                throw new ArgumentException("clientId is required", nameof(clientId));
            if (string.IsNullOrWhiteSpace(clientSecret))
                throw new ArgumentException("clientSecret is required", nameof(clientSecret));

            // Fast path: check without entering semaphore
            if (!string.IsNullOrEmpty(s_token) && DateTime.UtcNow < s_tokenExpiryUtc - s_tokenRefreshBefore)
                return s_token;

            await s_tokenSemaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                if (!string.IsNullOrEmpty(s_token) && DateTime.UtcNow < s_tokenExpiryUtc - s_tokenRefreshBefore)
                    return s_token;

                var tuple = await RequestAccessTokenFromServerAsync(clientId, clientSecret).ConfigureAwait(false);
                s_token = tuple.token;
                var expiresIn = tuple.expiresIn > 0 ? tuple.expiresIn : 3600L;
                s_tokenExpiryUtc = DateTime.UtcNow.AddSeconds(expiresIn);

                return s_token;
            }
            finally
            {
                s_tokenSemaphore.Release();
            }
        }

        /// <summary>
        /// Synchronous wrapper for AcquireAccessTokenAsync (keeps compatibility).
        /// </summary>
        public static string AcquireAccessToken(string clientId, string clientSecret)
        {
            return AcquireAccessTokenAsync(clientId, clientSecret).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Backwards-compatible method: acquire token and return client. Kept internal to discourage use.
        /// </summary>
        internal static OneClickLCAClient ClientFromCredentials(string clientId, string clientSecret)
        {
            var token = AcquireAccessToken(clientId, clientSecret);

            lock (s_clientLock)
            {
                if (s_client == null)
                    s_client = new OneClickLCAClient(token);
                else
                    s_client.SetBearerToken(token);

                return s_client;
            }
        }

        private static async Task<(string token, long expiresIn)> RequestAccessTokenFromServerAsync(string clientId, string clientSecret)
        {
            // Token endpoint as provided by One Click LCA
            var tokenEndpoint = "https://id.oneclicklcaapp.com/realms/oneclicklca/protocol/openid-connect/token";

            using (var http = new HttpClient())
            {
                var values = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
                };

                var content = new FormUrlEncodedContent(values);
                var resp = await http.PostAsync(tokenEndpoint, content).ConfigureAwait(false);
                var respContent = resp.Content == null ? string.Empty : await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!resp.IsSuccessStatusCode)
                    throw new HttpRequestException($"Failed to acquire access token. HTTP {(int)resp.StatusCode}: {resp.ReasonPhrase}. Response: {respContent}");

                var j = JObject.Parse(respContent);
                var token = j.Value<string>("access_token");
                var expires = j.Value<long?>("expires_in") ?? 0L;

                if (string.IsNullOrEmpty(token))
                    throw new InvalidOperationException("Access token not found in token endpoint response.");

                return (token, expires);
            }
        }
    }
}
