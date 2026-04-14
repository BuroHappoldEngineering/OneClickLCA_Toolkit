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
    [Description("Declared unit and physical properties from a materials carbon resource document (same JSON keys as on the search API document payload).")]
    public class MaterialsCarbonResourcePhysical : BHoMObject
    {
        [JsonPropertyName("unitForData")]
        [Description("Unit for reported data.")]
        public virtual string UnitForData { get; set; } = "";

        [JsonPropertyName("density")]
        [Description("Density.")]
        public virtual double? Density { get; set; }

        [JsonPropertyName("massConversionFactor")]
        [Description("Mass conversion factor.")]
        public virtual double? MassConversionFactor { get; set; }

        [JsonPropertyName("thermalLambda")]
        [Description("Thermal conductivity (lambda).")]
        public virtual double? ThermalLambda { get; set; }

        [JsonPropertyName("defaultThickness_mm")]
        [Description("Default thickness in millimetres.")]
        public virtual double? DefaultThickness_mm { get; set; }

        [JsonPropertyName("defaultThickness_in")]
        [Description("Default thickness in inches.")]
        public virtual double? DefaultThickness_in { get; set; }

        [JsonPropertyName("serviceLife")]
        [Description("Service life.")]
        public virtual double? ServiceLife { get; set; }

        [JsonPropertyName("impactNonLinear")]
        [Description("Whether impacts are non-linear.")]
        public virtual bool? ImpactNonLinear { get; set; }
    }
}
