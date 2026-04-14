/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace BH.oM.Adapters.OneClickLCA
{
    [Description("Resource and document identifiers from a materials carbon resource document (same JSON keys as on the search API document payload).")]
    public class MaterialsCarbonResourceIdentifiers : BHoMObject
    {
        [JsonPropertyName("_id")]
        [Description("Search document identifier (_id).")]
        public virtual string Id { get; set; } = "";

        [JsonPropertyName("resourceId")]
        [Description("Resource identifier.")]
        public virtual string ResourceId { get; set; } = "";

        [JsonPropertyName("profileId")]
        [Description("Profile identifier.")]
        public virtual string ProfileId { get; set; } = "";

        [JsonPropertyName("resourceType")]
        [Description("Resource type.")]
        public virtual string ResourceType { get; set; } = "";

        [JsonPropertyName("resourceSubType")]
        [Description("Resource sub-type.")]
        public virtual string ResourceSubType { get; set; } = "";

        [JsonPropertyName("resourceTypeL3")]
        [Description("Resource type level 3.")]
        public virtual string ResourceTypeL3 { get; set; } = "";

        [JsonPropertyName("epdNumber")]
        [Description("EPD number.")]
        public virtual string EpdNumber { get; set; } = "";
    }
}
