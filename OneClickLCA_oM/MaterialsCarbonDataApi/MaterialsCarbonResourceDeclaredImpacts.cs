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
    [Description("Declared-unit impact and biogenic storage scalars on a materials carbon resource document (same JSON keys as on the search API document payload).")]
    public class MaterialsCarbonResourceDeclaredImpacts : BHoMObject
    {
        [JsonPropertyName("impactGWP100_kgCO2e_biogenic_kg")]
        public virtual double? ImpactGWP100_kgCO2e_biogenic_kg { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_biogenic_lbs")]
        public virtual double? ImpactGWP100_kgCO2e_biogenic_lbs { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_biogenic_ton")]
        public virtual double? ImpactGWP100_kgCO2e_biogenic_ton { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_fossil_kg")]
        public virtual double? ImpactGWP100_kgCO2e_fossil_kg { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_fossil_lbs")]
        public virtual double? ImpactGWP100_kgCO2e_fossil_lbs { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_fossil_ton")]
        public virtual double? ImpactGWP100_kgCO2e_fossil_ton { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_kg")]
        public virtual double? ImpactGWP100_kgCO2e_kg { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_lbs")]
        public virtual double? ImpactGWP100_kgCO2e_lbs { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_luluc_kg")]
        public virtual double? ImpactGWP100_kgCO2e_luluc_kg { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_luluc_lbs")]
        public virtual double? ImpactGWP100_kgCO2e_luluc_lbs { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_luluc_ton")]
        public virtual double? ImpactGWP100_kgCO2e_luluc_ton { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_ton")]
        public virtual double? ImpactGWP100_kgCO2e_ton { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_total_kg")]
        public virtual double? ImpactGWP100_kgCO2e_total_kg { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_total_lbs")]
        public virtual double? ImpactGWP100_kgCO2e_total_lbs { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_total_ton")]
        public virtual double? ImpactGWP100_kgCO2e_total_ton { get; set; }

        [JsonPropertyName("traciGWP_kgCO2e_kg")]
        public virtual double? TraciGWP_kgCO2e_kg { get; set; }

        [JsonPropertyName("traciGWP_kgCO2e_lbs")]
        public virtual double? TraciGWP_kgCO2e_lbs { get; set; }

        [JsonPropertyName("traciGWP_kgCO2e_ton")]
        public virtual double? TraciGWP_kgCO2e_ton { get; set; }

        [JsonPropertyName("biogenicCarbonStorage_kgCO2e_cuft")]
        public virtual double? BiogenicCarbonStorage_kgCO2e_cuft { get; set; }

        [JsonPropertyName("biogenicCarbonStorage_kgCO2e_ft")]
        public virtual double? BiogenicCarbonStorage_kgCO2e_ft { get; set; }

        [JsonPropertyName("biogenicCarbonStorage_kgCO2e_kg")]
        public virtual double? BiogenicCarbonStorage_kgCO2e_kg { get; set; }

        [JsonPropertyName("biogenicCarbonStorage_kgCO2e_lbs")]
        public virtual double? BiogenicCarbonStorage_kgCO2e_lbs { get; set; }

        [JsonPropertyName("biogenicCarbonStorage_kgCO2e_m")]
        public virtual double? BiogenicCarbonStorage_kgCO2e_m { get; set; }

        [JsonPropertyName("biogenicCarbonStorage_kgCO2e_m2")]
        public virtual double? BiogenicCarbonStorage_kgCO2e_m2 { get; set; }

        [JsonPropertyName("biogenicCarbonStorage_kgCO2e_m3")]
        public virtual double? BiogenicCarbonStorage_kgCO2e_m3 { get; set; }

        [JsonPropertyName("biogenicCarbonStorage_kgCO2e_sqft")]
        public virtual double? BiogenicCarbonStorage_kgCO2e_sqft { get; set; }

        [JsonPropertyName("biogenicCarbonStorage_kgCO2e_ton")]
        public virtual double? BiogenicCarbonStorage_kgCO2e_ton { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_biogenic_cuft")]
        public virtual double? ImpactGWP100_kgCO2e_biogenic_cuft { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_biogenic_ft")]
        public virtual double? ImpactGWP100_kgCO2e_biogenic_ft { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_biogenic_m")]
        public virtual double? ImpactGWP100_kgCO2e_biogenic_m { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_biogenic_m2")]
        public virtual double? ImpactGWP100_kgCO2e_biogenic_m2 { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_biogenic_m3")]
        public virtual double? ImpactGWP100_kgCO2e_biogenic_m3 { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_biogenic_sqft")]
        public virtual double? ImpactGWP100_kgCO2e_biogenic_sqft { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_cuft")]
        public virtual double? ImpactGWP100_kgCO2e_cuft { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_fossil_cuft")]
        public virtual double? ImpactGWP100_kgCO2e_fossil_cuft { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_fossil_ft")]
        public virtual double? ImpactGWP100_kgCO2e_fossil_ft { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_fossil_m")]
        public virtual double? ImpactGWP100_kgCO2e_fossil_m { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_fossil_m2")]
        public virtual double? ImpactGWP100_kgCO2e_fossil_m2 { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_fossil_m3")]
        public virtual double? ImpactGWP100_kgCO2e_fossil_m3 { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_fossil_sqft")]
        public virtual double? ImpactGWP100_kgCO2e_fossil_sqft { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_ft")]
        public virtual double? ImpactGWP100_kgCO2e_ft { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_luluc_cuft")]
        public virtual double? ImpactGWP100_kgCO2e_luluc_cuft { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_luluc_ft")]
        public virtual double? ImpactGWP100_kgCO2e_luluc_ft { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_luluc_m")]
        public virtual double? ImpactGWP100_kgCO2e_luluc_m { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_luluc_m2")]
        public virtual double? ImpactGWP100_kgCO2e_luluc_m2 { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_luluc_m3")]
        public virtual double? ImpactGWP100_kgCO2e_luluc_m3 { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_luluc_sqft")]
        public virtual double? ImpactGWP100_kgCO2e_luluc_sqft { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_m")]
        public virtual double? ImpactGWP100_kgCO2e_m { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_m2")]
        public virtual double? ImpactGWP100_kgCO2e_m2 { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_m3")]
        public virtual double? ImpactGWP100_kgCO2e_m3 { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_sqft")]
        public virtual double? ImpactGWP100_kgCO2e_sqft { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_total_cuft")]
        public virtual double? ImpactGWP100_kgCO2e_total_cuft { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_total_ft")]
        public virtual double? ImpactGWP100_kgCO2e_total_ft { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_total_m")]
        public virtual double? ImpactGWP100_kgCO2e_total_m { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_total_m2")]
        public virtual double? ImpactGWP100_kgCO2e_total_m2 { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_total_m3")]
        public virtual double? ImpactGWP100_kgCO2e_total_m3 { get; set; }

        [JsonPropertyName("impactGWP100_kgCO2e_total_sqft")]
        public virtual double? ImpactGWP100_kgCO2e_total_sqft { get; set; }

        [JsonPropertyName("traciGWP_kgCO2e_cuft")]
        public virtual double? TraciGWP_kgCO2e_cuft { get; set; }

        [JsonPropertyName("traciGWP_kgCO2e_ft")]
        public virtual double? TraciGWP_kgCO2e_ft { get; set; }

        [JsonPropertyName("traciGWP_kgCO2e_m")]
        public virtual double? TraciGWP_kgCO2e_m { get; set; }

        [JsonPropertyName("traciGWP_kgCO2e_m2")]
        public virtual double? TraciGWP_kgCO2e_m2 { get; set; }

        [JsonPropertyName("traciGWP_kgCO2e_m3")]
        public virtual double? TraciGWP_kgCO2e_m3 { get; set; }

        [JsonPropertyName("traciGWP_kgCO2e_sqft")]
        public virtual double? TraciGWP_kgCO2e_sqft { get; set; }
    }
}
