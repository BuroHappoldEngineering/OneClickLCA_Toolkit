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
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace BH.oM.Adapters.OneClickLCA
{
    [Description("Resource document from the OneClick LCA materials carbon data search API (nested under each hit).")]
    public class MaterialsCarbonResourceDocument : BHoMObject
    {
        [JsonPropertyName("_id")]
        [Description("Document identifier.")]
        public virtual string Id { get; set; }

        [JsonPropertyName("areas")]
        public virtual List<string> Areas { get; set; } = new List<string>();

        [JsonPropertyName("combinedUnits")]
        public virtual List<string> CombinedUnits { get; set; } = new List<string>();

        [JsonPropertyName("dataProperties")]
        public virtual List<string> DataProperties { get; set; } = new List<string>();

        [JsonPropertyName("edited")]
        public virtual string Edited { get; set; }

        [JsonPropertyName("environmentDataPeriod")]
        public virtual double? EnvironmentDataPeriod { get; set; }

        [JsonPropertyName("environmentDataSource")]
        public virtual string EnvironmentDataSource { get; set; }

        [JsonPropertyName("environmentDataSourceStandard")]
        public virtual string EnvironmentDataSourceStandard { get; set; }

        [JsonPropertyName("environmentDataSourceType")]
        public virtual string EnvironmentDataSourceType { get; set; }

        [JsonPropertyName("epdNumber")]
        public virtual string EpdNumber { get; set; }

        [JsonPropertyName("epdProgram")]
        public virtual string EpdProgram { get; set; }

        [JsonPropertyName("firstUploadTime")]
        public virtual string FirstUploadTime { get; set; }

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

        [JsonPropertyName("impacts")]
        [Description("Per-module impact indicators (dynamic module keys such as A1-A3).")]
        public virtual Dictionary<string, MaterialsCarbonImpactModule> Impacts { get; set; } = new Dictionary<string, MaterialsCarbonImpactModule>();

        [JsonPropertyName("manufacturer")]
        public virtual string Manufacturer { get; set; }

        [JsonPropertyName("massConversionFactor")]
        public virtual double? MassConversionFactor { get; set; }

        [JsonPropertyName("nameEN")]
        public virtual string NameEN { get; set; }

        [JsonPropertyName("pcr")]
        public virtual string Pcr { get; set; }

        [JsonPropertyName("productDescription")]
        public virtual string ProductDescription { get; set; }

        [JsonPropertyName("profileId")]
        public virtual string ProfileId { get; set; }

        [JsonPropertyName("resistanceProperties")]
        public virtual List<string> ResistanceProperties { get; set; } = new List<string>();

        [JsonPropertyName("resourceId")]
        public virtual string ResourceId { get; set; }

        [JsonPropertyName("resourceSubType")]
        public virtual string ResourceSubType { get; set; }

        [JsonPropertyName("resourceType")]
        public virtual string ResourceType { get; set; }

        [JsonPropertyName("resourceTypeL3")]
        public virtual string ResourceTypeL3 { get; set; }

        [JsonPropertyName("staticFullName")]
        public virtual string StaticFullName { get; set; }

        [JsonPropertyName("traciGWP_kgCO2e_kg")]
        public virtual double? TraciGWP_kgCO2e_kg { get; set; }

        [JsonPropertyName("traciGWP_kgCO2e_lbs")]
        public virtual double? TraciGWP_kgCO2e_lbs { get; set; }

        [JsonPropertyName("traciGWP_kgCO2e_ton")]
        public virtual double? TraciGWP_kgCO2e_ton { get; set; }

        [JsonPropertyName("unitForData")]
        public virtual string UnitForData { get; set; }

        [JsonPropertyName("upstreamDB")]
        public virtual string UpstreamDB { get; set; }

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

        [JsonPropertyName("commercialName")]
        public virtual string CommercialName { get; set; }

        [JsonPropertyName("deactivateTime")]
        public virtual string DeactivateTime { get; set; }

        [JsonPropertyName("defaultThickness_in")]
        public virtual double? DefaultThickness_in { get; set; }

        [JsonPropertyName("defaultThickness_mm")]
        public virtual double? DefaultThickness_mm { get; set; }

        [JsonPropertyName("density")]
        public virtual double? Density { get; set; }

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

        [JsonPropertyName("impactNonLinear")]
        public virtual bool? ImpactNonLinear { get; set; }

        [JsonPropertyName("materialsCompassUrl")]
        public virtual string MaterialsCompassUrl { get; set; }

        [JsonPropertyName("nameCN")]
        public virtual string NameCN { get; set; }

        [JsonPropertyName("nameES")]
        public virtual string NameES { get; set; }

        [JsonPropertyName("nameHU")]
        public virtual string NameHU { get; set; }

        [JsonPropertyName("nameIT")]
        public virtual string NameIT { get; set; }

        [JsonPropertyName("nameJP")]
        public virtual string NameJP { get; set; }

        [JsonPropertyName("nameNL")]
        public virtual string NameNL { get; set; }

        [JsonPropertyName("resourceQualityWarning")]
        public virtual string ResourceQualityWarning { get; set; }

        [JsonPropertyName("resourceQualityWarningText")]
        public virtual string ResourceQualityWarningText { get; set; }

        [JsonPropertyName("serviceLife")]
        public virtual double? ServiceLife { get; set; }

        [JsonPropertyName("technicalSpec")]
        public virtual string TechnicalSpec { get; set; }

        [JsonPropertyName("thermalLambda")]
        public virtual double? ThermalLambda { get; set; }

        [JsonPropertyName("thirdPartyVerifier")]
        public virtual string ThirdPartyVerifier { get; set; }

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
