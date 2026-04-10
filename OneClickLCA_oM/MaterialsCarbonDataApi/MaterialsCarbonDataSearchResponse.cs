/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
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

using BH.oM.Base;
using BH.oM.Base.Attributes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BH.oM.Adapters.OneClickLCA
{
    [Description("Response from POST materials-carbon-data resource search (one page or aggregated across adapter pagination).")]
    public class MaterialsCarbonDataSearchResponse : BHoMObject
    {
        [JsonPropertyName("facet_counts")]
        [Description("Facet counts returned by the search service (structure varies).")]
        public virtual List<JsonElement> FacetCounts { get; set; } = new List<JsonElement>();

        [JsonPropertyName("found")]
        [Description("Total number of matching resources reported by the API for the query.")]
        public virtual int Found { get; set; }

        [JsonPropertyName("hits")]
        [Description("Search hits for this response (combined across pages when returned from Pull).")]
        public virtual List<MaterialsCarbonSearchHit> Hits { get; set; } = new List<MaterialsCarbonSearchHit>();

        [JsonPropertyName("page")]
        [Description("Page number of this payload, or the last page fetched when aggregated.")]
        public virtual int Page { get; set; }

        [JsonPropertyName("request_params")]
        [Description("Echo of request parameters from the search API.")]
        public virtual MaterialsCarbonRequestParams RequestParams { get; set; }

        [JsonPropertyName("search_cutoff")]
        [Description("Whether the search was cut off by the service.")]
        public virtual bool SearchCutoff { get; set; }

        [JsonPropertyName("search_time_ms")]
        [Description("Search duration in milliseconds for this request.")]
        public virtual long SearchTimeMs { get; set; }
    }
}
