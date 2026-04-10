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
    [Description("Response from GET /projects: list of projects with pagination and optional notifications.")]
    public class ProjectListResponse : BHoMObject
    {
        [JsonPropertyName("warning")]
        [Description("Optional warning notification from the API.")]
        public virtual ApiNotification Warning { get; set; }

        [JsonPropertyName("info")]
        [Description("Optional info notification from the API.")]
        public virtual ApiNotification Info { get; set; }

        [JsonPropertyName("projects")]
        [Description("List of projects with associated designs.")]
        public virtual List<Project> Projects { get; set; } = new List<Project>();

        [JsonPropertyName("pagination")]
        [Description("Pagination metadata.")]
        public virtual PaginationInfo Pagination { get; set; }
    }

    [Description("Notification (info, warning or error) from the OneClick LCA API.")]
    public class ApiNotification : BHoMObject
    {
        [JsonPropertyName("level")]
        public virtual string Level { get; set; } = "";

        [JsonPropertyName("code")]
        public virtual string Code { get; set; } = "";

        [JsonPropertyName("message")]
        public virtual string Message { get; set; } = "";

        [JsonPropertyName("description")]
        public virtual string Description { get; set; } = "";
    }
}
