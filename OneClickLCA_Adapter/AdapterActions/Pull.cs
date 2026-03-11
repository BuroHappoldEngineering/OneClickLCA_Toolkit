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
using BH.Adapter.Excel;
using BH.Engine.Adapter;
using BH.Engine.Base;
using BH.oM.Adapter;
using BH.oM.Adapters.Excel;
using BH.oM.Adapters.OneClickLCA;
using BH.oM.Base;
using BH.oM.Data.Requests;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Text.Json;

namespace BH.Adapter.OneClickLCA
{
    public partial class OneClickLCAAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Method Overrides                          ****/
        /***************************************************/

        public override IEnumerable<object> Pull(IRequest request = null, PullType pullOption = PullType.AdapterDefault, ActionConfig actionConfig = null)
        {
            return _Pull(request as dynamic);
        }


        /***************************************************/
        /**** Private Methods — Excel Report           ****/
        /***************************************************/

        private IEnumerable<object> _Pull(ReportRequest request)
        {
            string fileExtension = Path.GetExtension(request.FileName);

            if (fileExtension == ".xls")
                return PullFromXls(request);
            else
                return PullFromXlsx(request);
        }

        /***************************************************/

        private IEnumerable<object> PullFromXls(ReportRequest request)
        {
            // Read the Excel file
            Dictionary<string, object> metadata = new Dictionary<string, object>();
            List<string> headers = new List<string>();
            List<List<string>> content = new List<List<string>>();
            try
            {
                using (FileStream fileStream = new FileStream(Path.Combine(request.Directory, request.FileName), FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(fileStream))
                    {
                        // Get metadata
                        metadata = GetMetaData(reader);

                        // Get headers
                        reader.Read();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string header = reader.GetString(i);
                            if (header == null)
                                header = "Column" + i;
                            headers.Add(header);
                        }
                            

                        // Get the content
                        while (reader.Read())
                        {
                            List<string> row = new List<string>();
                            for (int i = 0; i < reader.FieldCount; i++)
                                row.Add(reader.GetValue(i)?.ToString());
                            content.Add(row);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                BH.Engine.Base.Compute.RecordError($"Failed to read file {request.FileName}. Error: {e.Message}");
            }

            // Create the report with its metadata
            OneClickReport report = FillReportMetadata(metadata);

            // Attach any potential additional inputs to the report
            if (request.AdditionalInputs != null)
                report.Fragments.Add(request.AdditionalInputs);

            return new List<object> { PopulateReport(report, headers, content) };
        }

        /***************************************************/

        private IEnumerable<object> PullFromXlsx(ReportRequest request)
        {
            ExcelAdapter excelAdapter = new ExcelAdapter(new FileSettings { FileName = request.FileName, Directory = request.Directory });

            // Pull the meta data
            CustomObject metadata = excelAdapter.Pull(BH.Engine.Excel.Create.ObjectRequest("", "A1:D2")).FirstOrDefault() as CustomObject;
            if (metadata == null)
            {
                BH.Engine.Base.Compute.RecordError("Failed to pull the metadata from the report. Make sure that the file is in the correct location.");
                return new List<object>();
            }
            OneClickReport report = FillReportMetadata(metadata.CustomData);

            // Attach any potential additional inputs to the report
            if (request.AdditionalInputs != null)
                report.Fragments.Add(request.AdditionalInputs);

            // Get the valid range of cells by looking at the column headers
            TableRow headers = excelAdapter.Pull(BH.Engine.Excel.Create.CellValuesRequest("", "A3:AZ3")).FirstOrDefault() as TableRow;
            if (headers == null || headers.Content?.Count == 0)
            {
                BH.Engine.Base.Compute.RecordError("Failed to pull the column headers from the report. Make sure you provide the correct file.");
                return new List<object>();
            }
            int nbRows = headers.Content.FindIndex(x => string.IsNullOrWhiteSpace(x?.ToString()));
            string cellRange = $"A4:{BH.Engine.Excel.Query.ColumnName(nbRows - 1)}";
            headers.Content = headers.Content.Take(nbRows).ToList();

            // Get the report content
            List<TableRow> content = excelAdapter.Pull(BH.Engine.Excel.Create.CellValuesRequest("", cellRange)).OfType<TableRow>().ToList();
            if (content?.Count == 0)
            {
                BH.Engine.Base.Compute.RecordError("Failed to pull the content from the report. Make sure you provide the correct file.");
                return new List<object>();
            }

            return new List<object> { PopulateReport(report, headers, content) };
        }

        /***************************************************/

        private OneClickReport FillReportMetadata(Dictionary<string, object> metadata)
        {
            OneClickReport report = new OneClickReport();

            if (metadata.ContainsKey("Entity users"))
                report.EntityUsers = metadata["Entity users"].ToString().Split(',').ToList();

            if (metadata.ContainsKey("Project name"))
                report.ProjectName = metadata["Project name"].ToString();

            if (metadata.ContainsKey("Design name"))
                report.DesignName = metadata["Design name"].ToString();

            if (metadata.ContainsKey("Indicator name"))
                report.Indicator = BH.Engine.Base.Compute.ParseEnum<Indicator>(metadata["Indicator name"].ToString());

            return report;
        }

        /***************************************************/

        private Dictionary<string, object> GetMetaData(IExcelDataReader reader)
        {
            List<string> keys = new List<string>();
            reader.Read();
            for (int i = 0; i < 4; i++)
                keys.Add(reader.GetString(i));

            List<string> values = new List<string>();
            reader.Read();
            for (int i = 0; i < 4; i++)
                values.Add(reader.GetString(i));

            return keys.Zip(values, (string k, object v) => new { k, v }).ToDictionary(x => x.k, x => x.v);
        }


        /***************************************************/
        /**** Private Methods — Carbon Data API        ****/
        /***************************************************/

        private IEnumerable<object> _Pull(CarbonDataApiRequest request)
        {
            if (string.IsNullOrEmpty(request.ClientId) || string.IsNullOrEmpty(request.ClientSecret))
            {
                BH.Engine.Base.Compute.RecordError("Client ID and Client Secret are required for the OneClick LCA Carbon Data API.");
                return new List<object>();
            }

            string token = AcquireToken(request.ClientId, request.ClientSecret);
            if (token == null)
                return new List<object>();

            List<string> documents = SearchResources(token, request);

            return documents
                .Select(d => Convert.ToEnvironmentalProductDeclaration(d))
                .Where(epd => epd != null)
                .Cast<object>()
                .ToList();
        }

        /***************************************************/

        private string AcquireToken(string clientId, string clientSecret)
        {
            const string tokenUrl = "https://id.oneclicklcaapp.com/realms/oneclicklca/protocol/openid-connect/token";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Prepare client credentials form
                    FormUrlEncodedContent body = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "grant_type", "client_credentials" },
                        { "client_id", clientId },
                        { "client_secret", clientSecret }
                    });

                    HttpResponseMessage response = client.PostAsync(tokenUrl, body).Result;

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
                BH.Engine.Base.Compute.RecordError($"Failed to acquire OneClick LCA access token. Error: {e.Message}");
                return null;
            }
        }

        /***************************************************/

        private List<string> SearchResources(string token, CarbonDataApiRequest request)
        {
            const string searchUrl = "https://oneclicklcaapp.com/api/materials-carbon-data/resource/_search";
            const int perPage = 250;

            List<string> documents = new List<string>();
            int page = 1;
            int totalAvailable = int.MaxValue;

            while (documents.Count < request.MaxResults && documents.Count < totalAvailable)
            {
                int remaining = Math.Min(perPage, request.MaxResults - documents.Count);

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "q",        string.IsNullOrEmpty(request.SearchQuery) ? "*" : request.SearchQuery },
                    { "page",     page },
                    { "per_page", remaining }
                };

                if (!string.IsNullOrEmpty(request.QueryBy))
                    parameters["query_by"] = request.QueryBy;

                if (!string.IsNullOrEmpty(request.FilterBy))
                    parameters["filter_by"] = request.FilterBy;

                if (!string.IsNullOrEmpty(request.SortBy))
                    parameters["sort_by"] = request.SortBy;

                string responseJson = BH.Engine.Adapters.HTTP.Compute.MakeRequest(new BH.oM.Adapters.HTTP.GetRequest
                {
                    BaseUrl = searchUrl,
                    Headers = new Dictionary<string, object> { { "Authorization", $"Bearer {token}" } },
                    Parameters = parameters
                });

                if (responseJson == null)
                    break;

                try
                {
                    using (JsonDocument doc = JsonDocument.Parse(responseJson))
                    {
                        JsonElement root = doc.RootElement;

                        if (root.TryGetProperty("found", out JsonElement found))
                            totalAvailable = found.GetInt32();

                        if (!root.TryGetProperty("hits", out JsonElement hits) || hits.GetArrayLength() == 0)
                            break;

                        foreach (JsonElement hit in hits.EnumerateArray())
                        {
                            if (hit.TryGetProperty("document", out JsonElement document))
                                documents.Add(document.GetRawText());
                        }
                    }
                }
                catch (JsonException e)
                {
                    BH.Engine.Base.Compute.RecordError($"Failed to parse search response from OneClick LCA API. Error: {e.Message}");
                    break;
                }

                page++;
            }

            return documents;
        }


        /***************************************************/
        /**** Fallback Methods                          ****/
        /***************************************************/

        private IEnumerable<object> _Pull(IRequest request)
        {
            if (request == null)
                BH.Engine.Base.Compute.RecordError($"Please provide a valid request for the Pull to work correctly.");
            else
                BH.Engine.Base.Compute.RecordError($"Only requests of type {request.GetType()} are not supported.");

            return new List<IBHoMObject>();
        }

        /***************************************************/
    }
}





