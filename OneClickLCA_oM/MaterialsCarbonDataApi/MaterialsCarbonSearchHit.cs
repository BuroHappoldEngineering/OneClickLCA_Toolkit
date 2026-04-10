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
    [Description("One search hit from the materials carbon data API (document plus optional highlight metadata).")]
    public class MaterialsCarbonSearchHit : BHoMObject
    {
        [JsonPropertyName("document")]
        [Description("Resource document payload for this hit.")]
        public virtual MaterialsCarbonResourceDocument Document { get; set; }

        [JsonPropertyName("highlight")]
        [Description("Search highlight payload from Typesense (structure varies).")]
        public virtual JsonElement Highlight { get; set; }

        [JsonPropertyName("highlights")]
        [Description("Per-field highlights when returned by the API.")]
        public virtual List<JsonElement> Highlights { get; set; } = new List<JsonElement>();
    }
}
