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
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace BH.oM.Adapters.OneClickLCA
{
    [Description("Environmental impact values for one life-cycle module within a materials carbon resource document (nested under impacts).")]
    public class MaterialsCarbonImpactModule : BHoMObject
    {
        [JsonPropertyName("impactGWP100_kgCO2e")]
        [Description("Climate change (GWP100) excluding biogenic CO2, kg CO2e per declared unit.")]
        public virtual double? ImpactGWP100_kgCO2e { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_total")]
        [Description("Total climate change (GWP100) including biogenic, kg CO2e per declared unit.")]
        public virtual double? ImpactGWP100_kgCO2e_total { get; set; }

        [JsonPropertyName("traciGWP_kgCO2e")]
        [Description("TRACI global warming potential, kg CO2e per declared unit.")]
        public virtual double? TraciGWP_kgCO2e { get; set; }
    }
}
