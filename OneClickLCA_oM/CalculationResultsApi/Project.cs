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
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace BH.oM.Adapters.OneClickLCA
{
    [Description("Project from the OneClick LCA Calculation Results API, with associated designs and available tools.")]
    public class Project : BHoMObject
    {
        [JsonPropertyName("projectId")]
        [Description("Unique identifier of the project.")]
        public virtual string ProjectId { get; set; } = "";

        [JsonPropertyName("name")]
        [Description("Name of the project.")]
        public new virtual string Name { get; set; } = "";

        [JsonPropertyName("projectType")]
        [Description("Type of the project (e.g. building, infrastructure, buildingProduct).")]
        public virtual string ProjectType { get; set; } = "";

        [JsonPropertyName("assetType")]
        [Description("Type of the building or product.")]
        public virtual string AssetType { get; set; } = "";

        [JsonPropertyName("dateCreated")]
        [Description("Date created in ISO 8601 UTC format.")]
        public virtual string DateCreated { get; set; } = "";

        [JsonPropertyName("lastUpdated")]
        [Description("Last updated date and time in ISO 8601 UTC format.")]
        public virtual string LastUpdated { get; set; } = "";

        [JsonPropertyName("availableTools")]
        [Description("List of available tool identifiers for this project.")]
        public virtual List<string> AvailableTools { get; set; } = new List<string>();

        [JsonPropertyName("designs")]
        [Description("List of designs associated with the project.")]
        public virtual List<Design> Designs { get; set; } = new List<Design>();

        [JsonPropertyName("country")]
        [Description("Name of the country of the project.")]
        public virtual string Country { get; set; } = "";

        [JsonPropertyName("projectCode")]
        [Description("Project code that belongs to the project.")]
        public virtual string ProjectCode { get; set; } = "";

        [JsonPropertyName("createdBy")]
        [Description("User who created the project.")]
        public virtual BasicUserDetails CreatedBy { get; set; }

        [JsonPropertyName("lastUpdatedBy")]
        [Description("User who last updated the project.")]
        public virtual BasicUserDetails LastUpdatedBy { get; set; }

        [JsonPropertyName("primaryTool")]
        [Description("Primary tool selected for the project.")]
        public virtual string PrimaryTool { get; set; } = "";
    }
}
