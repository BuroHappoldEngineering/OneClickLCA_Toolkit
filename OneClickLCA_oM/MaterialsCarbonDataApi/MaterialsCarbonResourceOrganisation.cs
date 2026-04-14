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
    [Description("Organisation, verification, classification, and product text from a materials carbon resource document (same JSON keys as on the search API document payload).")]
    public class MaterialsCarbonResourceOrganisation : BHoMObject
    {
        [JsonPropertyName("manufacturer")]
        [Description("Manufacturer.")]
        public virtual string Manufacturer { get; set; } = "";

        [JsonPropertyName("epdProgram")]
        [Description("EPD programme.")]
        public virtual string EpdProgram { get; set; } = "";

        [JsonPropertyName("thirdPartyVerifier")]
        [Description("Third-party verifier.")]
        public virtual string ThirdPartyVerifier { get; set; } = "";

        [JsonPropertyName("materialsCompassUrl")]
        [Description("Materials Compass URL.")]
        public virtual string MaterialsCompassUrl { get; set; } = "";

        [JsonPropertyName("technicalSpec")]
        [Description("Technical specification text.")]
        public virtual string TechnicalSpec { get; set; } = "";

        [JsonPropertyName("resourceQualityWarning")]
        [Description("Resource quality warning code or level.")]
        public virtual string ResourceQualityWarning { get; set; } = "";

        [JsonPropertyName("resourceQualityWarningText")]
        [Description("Resource quality warning text.")]
        public virtual string ResourceQualityWarningText { get; set; } = "";

        [JsonPropertyName("productDescription")]
        [Description("Product description.")]
        public virtual string ProductDescription { get; set; } = "";

        [JsonPropertyName("areas")]
        [Description("Geographic or market areas.")]
        public virtual List<string> Areas { get; set; } = new List<string>();

        [JsonPropertyName("dataProperties")]
        [Description("Data property tags.")]
        public virtual List<string> DataProperties { get; set; } = new List<string>();

        [JsonPropertyName("resistanceProperties")]
        [Description("Resistance property tags.")]
        public virtual List<string> ResistanceProperties { get; set; } = new List<string>();

        [JsonPropertyName("combinedUnits")]
        [Description("Combined units.")]
        public virtual List<string> CombinedUnits { get; set; } = new List<string>();
    }
}
