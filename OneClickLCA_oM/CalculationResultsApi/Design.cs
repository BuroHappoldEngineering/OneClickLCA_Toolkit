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
    [Description("Design associated with a project from the OneClick LCA Calculation Results API.")]
    public class Design : BHoMObject
    {
        [JsonPropertyName("designId")]
        [Description("Unique identifier of the design (24-character alphanumeric).")]
        public virtual string DesignId { get; set; } = "";

        [JsonPropertyName("name")]
        [Description("Name of the design.")]
        public new virtual string Name { get; set; } = "";

        [JsonPropertyName("designType")]
        [Description("Design type (e.g. design, operatingPeriod, carbonDesign).")]
        public virtual string DesignType { get; set; } = "";

        [JsonPropertyName("chosenDesign")]
        [Description("Whether this design is set as the latest status.")]
        public virtual bool ChosenDesign { get; set; }

        [JsonPropertyName("baseline")]
        [Description("Whether this is a baseline design of the project.")]
        public virtual bool Baseline { get; set; }

        [JsonPropertyName("ribaStage")]
        [Description("Stage of construction process (RIBA / AIA stages).")]
        public virtual string RibaStage { get; set; } = "";

        [JsonPropertyName("lastUpdated")]
        [Description("Last updated date and time in ISO 8601 UTC format.")]
        public virtual string LastUpdated { get; set; } = "";

        [JsonPropertyName("createdBy")]
        [Description("User who created the design.")]
        public virtual BasicUserDetails CreatedBy { get; set; }

        [JsonPropertyName("lastUpdatedBy")]
        [Description("User who last updated the design.")]
        public virtual BasicUserDetails LastUpdatedBy { get; set; }
    }
}
