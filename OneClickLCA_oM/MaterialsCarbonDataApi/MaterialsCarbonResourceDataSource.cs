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
    [Description("Data source, programme, and lifecycle metadata from a materials carbon resource document (same JSON keys as on the search API document payload).")]
    public class MaterialsCarbonResourceDataSource : BHoMObject
    {
        [JsonPropertyName("environmentDataSource")]
        [Description("Environment data source.")]
        public virtual string EnvironmentDataSource { get; set; } = "";

        [JsonPropertyName("environmentDataSourceStandard")]
        [Description("Environment data source standard.")]
        public virtual string EnvironmentDataSourceStandard { get; set; } = "";

        [JsonPropertyName("environmentDataSourceType")]
        [Description("Environment data source type.")]
        public virtual string EnvironmentDataSourceType { get; set; } = "";

        [JsonPropertyName("environmentDataPeriod")]
        [Description("Environment data period.")]
        public virtual double? EnvironmentDataPeriod { get; set; }

        [JsonPropertyName("pcr")]
        [Description("Product category rules (PCR) reference.")]
        public virtual string Pcr { get; set; } = "";

        [JsonPropertyName("upstreamDB")]
        [Description("Upstream database reference.")]
        public virtual string UpstreamDB { get; set; } = "";

        [JsonPropertyName("edited")]
        [Description("Edited timestamp or flag from the API.")]
        public virtual string Edited { get; set; } = "";

        [JsonPropertyName("firstUploadTime")]
        [Description("First upload time.")]
        public virtual string FirstUploadTime { get; set; } = "";

        [JsonPropertyName("deactivateTime")]
        [Description("Deactivate time.")]
        public virtual string DeactivateTime { get; set; } = "";
    }
}
