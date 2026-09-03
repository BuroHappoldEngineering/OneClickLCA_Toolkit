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

namespace BH.oM.Adapters.OneClickLCA
{
    [Description("Resolved calculation-results dictionary for one tool: design identifier, tool identifier, and rule/category display name maps. Values are keyed by tool id in the dictionary returned from Engine MapDictionaryDataByToolId.")]
    public class DesignToolDictionaryMapping : BHoMObject
    {
        [Description("Design identifier from the dictionary payload.")]
        public virtual string DesignId { get; set; } = "";

        [Description("Tool identifier (e.g. flexibleEPDTool, lcaRicsV2).")]
        public virtual string ToolId { get; set; } = "";

        [Description("Human-readable tool name from the API tool map.")]
        public virtual string ToolDisplayName { get; set; } = "";

        [Description("Calculation rule identifier to display name for this tool.")]
        public virtual Dictionary<string, string> Rules { get; set; } = new Dictionary<string, string>();

        [Description("Result category identifier to display name for this tool.")]
        public virtual Dictionary<string, string> Categories { get; set; } = new Dictionary<string, string>();
    }
}
