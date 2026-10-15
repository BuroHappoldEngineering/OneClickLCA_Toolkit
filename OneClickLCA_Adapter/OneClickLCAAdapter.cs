/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using BH.Adapter;
using BH.Adapter.HTTP;
using BH.Adapter.SQL;
using BH.oM.Adapter;
using BH.oM.Base.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace BH.Adapter.OneClickLCA
{
    public partial class OneClickLCAAdapter : BHoMAdapter
    {
        private TimeSpan DefaultTimeout = TimeSpan.FromMinutes(2);

        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        [Description("Adapter for OneClickLCA. Supports reading OneClick LCA report files (Excel) and querying the OneClick LCA Materials Carbon Data API.")]
        [Output("The created OneClickLCA adapter.")]
        public OneClickLCAAdapter(HttpClient apiClient = null)
        {
            m_AdapterSettings.DefaultPushType = oM.Adapter.PushType.CreateNonExisting;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            if (apiClient is null)
                apiClient = new HttpClient() { Timeout = DefaultTimeout };
            m_apiAdapter = new HTTPAdapter(apiClient);

            SetUpUserAdapter("SQL-BHOM01", "OneClickLCA_Production");
        }

        /***************************************************/

        [Description("Adapter for OneClickLCA. Supports reading OneClick LCA report files (Excel) and querying the OneClick LCA Materials Carbon Data API.")]
        [Input("server", "The server name to connect to the authentication database.")]
        [Input("database", "The database name to connect to the authentication database.")]
        [Output("The created OneClickLCA adapter.")]
        public OneClickLCAAdapter(string server, string database, HttpClient apiClient = null)
        {
            m_AdapterSettings.DefaultPushType = oM.Adapter.PushType.CreateNonExisting;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            if (apiClient is null)
                apiClient = new HttpClient() { Timeout = DefaultTimeout };
            m_apiAdapter = new HTTPAdapter(apiClient);

            SetUpUserAdapter(server, database);
        }

        /***************************************************/

        [Description("Adapter for OneClickLCA. Supports reading OneClick LCA report files (Excel) and querying the OneClick LCA Materials Carbon Data API.")]
        [Input("connectionString", "The connection string to connect to the authentication database.")]
        [Output("The created OneClickLCA adapter.")]
        public OneClickLCAAdapter(string connectionString, HttpClient apiClient = null)
        {
            m_AdapterSettings.DefaultPushType = oM.Adapter.PushType.CreateNonExisting;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            if (apiClient is null)
                apiClient = new HttpClient() { Timeout = DefaultTimeout };
            m_apiAdapter = new HTTPAdapter(apiClient);

            SetUpUserAdapter(connectionString);
        }

        /***************************************************/

        [Description("Adapter for OneClickLCA using a custom user adapter. If you are unsure of how to use this, try using the `OneClickLCAAdapter` constructor with no inputs.")]
        [Input("userAdapter", "The IBHoMAdapter that connects to a valid authentication method.")]
        [Output("The created OneClickLCA adapter.")]
        public OneClickLCAAdapter(IBHoMAdapter userAdapter, IBHoMAdapter apiAdapter)
        {
            m_AdapterSettings.DefaultPushType = oM.Adapter.PushType.CreateNonExisting;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            m_userAdapter = userAdapter;
            m_apiAdapter = apiAdapter;
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private void SetUpUserAdapter(string connectionString)
        {
            SqlAdapter userAdapter = new SqlAdapter(connectionString);
            m_userAdapter = userAdapter;
        }

        /***************************************************/

        private void SetUpUserAdapter(string server, string database)
        {
            SqlAdapter userAdapter = new SqlAdapter(server, database);
            m_userAdapter = userAdapter;
        }

        /***************************************************/

        private string AcquireToken(string clientId, string clientSecret)
        {
            const string tokenUrl = "https://id.oneclicklcaapp.com/realms/oneclicklca/protocol/openid-connect/token";

            if (!(m_apiAdapter is HTTPAdapter adapter))
                return "";

            try
            {
                // Prepare client credentials form
                FormUrlEncodedContent body = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" },
                    { "client_id", clientId },
                    { "client_secret", clientSecret }
                });

                using (HttpResponseMessage response = adapter.HttpClient.PostAsync(tokenUrl, body).Result)
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        BH.Engine.Base.Compute.RecordError($"Failed to acquire OneClick LCA access token. Response: {(int)response.StatusCode} {response.ReasonPhrase}");
                        return null;
                    }

                    string json = response.Content.ReadAsStringAsync().Result;

                    using (JsonDocument doc = JsonDocument.Parse(json))
                    {
                        if (doc.RootElement.TryGetProperty("access_token", out JsonElement tokenElement))
                            return tokenElement.GetString();
                    }

                    BH.Engine.Base.Compute.RecordError("Failed to extract access token from the OneClick LCA authentication response.");
                    return null;
                }
            }
            catch (Exception e)
            {
                BH.Engine.Base.Compute.RecordError(e, "Failed to acquire OneClick LCA access token.");
                return null;
            }
        }

        /***************************************************/
        /**** Private  Fields                           ****/
        /***************************************************/

        //using `IBHoMAdapter` interface to allow testing with a stub adapter that always returns the API key.
        //the important thing is that a `TableRequest` sent with `Pull` with the correct table and filter should return at least one user.
        //this is a form of Dependency Injection (https://en.wikipedia.org/wiki/Dependency_injection)
        IBHoMAdapter m_userAdapter;
        //ditto but with requests to one click api
        IBHoMAdapter m_apiAdapter;

        /***************************************************/
    }
}


