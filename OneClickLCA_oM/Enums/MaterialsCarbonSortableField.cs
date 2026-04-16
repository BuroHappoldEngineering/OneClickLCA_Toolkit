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

using BH.oM.Base.Attributes;
using System.ComponentModel;

namespace BH.oM.Adapters.OneClickLCA
{
    [Description("Fields supported for the Materials Carbon Data API sort_by parameter (see customer documentation). Use Engine Compute SortBy / SortByMany to build a sort_by string.")]
    public enum MaterialsCarbonSortableField
    {
        Undefined,
        DeactivateTime,
        Edited,
        EnvironmentDataPeriod,
        FirstUploadTime,
        Manufacturer
    }
}
