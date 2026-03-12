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
using BH.oM.Data.Requests;
using System.ComponentModel;

namespace BH.oM.Adapters.OneClickLCA
{
    [Description("Request calculation results for a design and tool from the OneClick LCA Calculation Results API (GET /calculation-results). For projects use GetProjectsRequest; for dictionary data use GetDictionaryDataRequest.")]
    public class CalculationResultsApiRequest : BHoMObject, IRequest
    {
        [Description("OAuth2 client identifier used for client credentials authentication.")]
        public virtual string ClientId { get; set; } = "";

        [Description("OAuth2 client secret used for client credentials authentication.")]
        public virtual string ClientSecret { get; set; } = "";

        [Description("Unique identifier for the design (24-character alphanumeric).")]
        public virtual string DesignId { get; set; } = "";

        [Description("Identifier for the tool used in the calculation (e.g. flexibleEPDTool, simplifiedLifeCycleCarbon).")]
        public virtual string ToolId { get; set; } = "";

        [Description("When true, include all result categories for the tool including dummy categories not shown on the result page. Defaults to false.")]
        public virtual bool ShowAllCategoriesForTool { get; set; } = false;
    }
}
