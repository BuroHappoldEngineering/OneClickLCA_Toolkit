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
    [Description("Request the list of projects with associated designs from the OneClick LCA Calculation Results API (GET /projects). Pull returns a single ProjectDataApiResponse whose Projects list aggregates pages up to MaxResults.")]
    public class ProjectsDataApiRequest : BHoMObject, IRequest
    {
        [Description("OAuth2 client identifier used for client credentials authentication.")]
        public virtual string ClientId { get; set; } = "";

        [Description("OAuth2 client secret used for client credentials authentication.")]
        public virtual string ClientSecret { get; set; } = "";

        [Description("Page number. Defaults to 1.")]
        public virtual int Page { get; set; } = 1;

        [Description("Number of records per page (max 100). Defaults to 20.")]
        public virtual int Limit { get; set; } = 20;

        [Description("Filter projects updated after this date (UTC). Supported formats: yyyy-MM-dd, M/d/yy, dd.MM.yyyy.")]
        public virtual string LastUpdatedAfter { get; set; } = "";

        [Description("Maximum number of items to pull when paginating. Defaults to 250.")]
        public virtual int MaxResults { get; set; } = 250;
    }
}
