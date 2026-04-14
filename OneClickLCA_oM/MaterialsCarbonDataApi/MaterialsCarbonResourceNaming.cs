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
    [Description("Naming fields from a materials carbon resource document (same JSON keys as on the search API document payload).")]
    public class MaterialsCarbonResourceNaming : BHoMObject
    {
        [JsonPropertyName("staticFullName")]
        [Description("Static full name.")]
        public virtual string StaticFullName { get; set; } = "";

        [JsonPropertyName("nameEN")]
        [Description("English name.")]
        public virtual string NameEN { get; set; } = "";

        [JsonPropertyName("commercialName")]
        [Description("Commercial name.")]
        public virtual string CommercialName { get; set; } = "";

        [JsonPropertyName("nameCN")]
        [Description("Chinese name.")]
        public virtual string NameCN { get; set; } = "";

        [JsonPropertyName("nameES")]
        [Description("Spanish name.")]
        public virtual string NameES { get; set; } = "";

        [JsonPropertyName("nameHU")]
        [Description("Hungarian name.")]
        public virtual string NameHU { get; set; } = "";

        [JsonPropertyName("nameIT")]
        [Description("Italian name.")]
        public virtual string NameIT { get; set; } = "";

        [JsonPropertyName("nameJP")]
        [Description("Japanese name.")]
        public virtual string NameJP { get; set; } = "";

        [JsonPropertyName("nameNL")]
        [Description("Dutch name.")]
        public virtual string NameNL { get; set; } = "";
    }
}
