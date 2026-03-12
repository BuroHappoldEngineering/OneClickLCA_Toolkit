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
    [Description("User details from the OneClick LCA Calculation Results API (e.g. createdBy, lastUpdatedBy).")]
    public class BasicUserDetails : BHoMObject
    {
        [JsonPropertyName("userId")]
        [Description("Unique identifier of the user.")]
        public virtual string UserId { get; set; } = "";

        [JsonPropertyName("email")]
        [Description("Email address of the user.")]
        public virtual string Email { get; set; } = "";

        [JsonPropertyName("name")]
        [Description("Display name of the user.")]
        public new virtual string Name { get; set; } = "";
    }
}
