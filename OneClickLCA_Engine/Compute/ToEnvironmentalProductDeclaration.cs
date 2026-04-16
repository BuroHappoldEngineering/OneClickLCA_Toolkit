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

using BH.oM.Adapters.OneClickLCA;
using BH.oM.Base.Attributes;
using BH.oM.LifeCycleAssessment;
using BH.oM.LifeCycleAssessment.Fragments;
using BH.oM.LifeCycleAssessment.MaterialFragments;
using Module = BH.oM.LifeCycleAssessment.Module;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BH.Engine.Adapters.OneClickLCA
{
    public static partial class Compute
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Creates an EnvironmentalProductDeclaration from raw materials carbon resource document JSON.")]
        [Input("json", "JSON object for a single resource document from the materials carbon data API.")]
        [Output("epd", "Environmental product declaration populated from the document.")]
        public static EnvironmentalProductDeclaration ToEnvironmentalProductDeclaration(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                BH.Engine.Base.Compute.RecordError("Resource document JSON is null or empty.");
                return null;
            }

            JsonDocument doc;
            try
            {
                doc = JsonDocument.Parse(json);
            }
            catch (JsonException e)
            {
                BH.Engine.Base.Compute.RecordError($"Failed to parse resource document JSON. Error: {e.Message}");
                return null;
            }

            using (doc)
            {
                return EnvironmentalProductDeclarationFromJsonElement(doc.RootElement);
            }
        }

        [Description("Creates an EnvironmentalProductDeclaration from a typed materials carbon resource document.")]
        [Input("document", "Resource document deserialized from the materials carbon data API.")]
        [Output("epd", "Environmental product declaration populated from the document.")]
        public static EnvironmentalProductDeclaration ToEnvironmentalProductDeclaration(MaterialsCarbonResourceDocument document)
        {
            if (document == null)
            {
                BH.Engine.Base.Compute.RecordError("Resource document is null.");
                return null;
            }

            try
            {
                return EnvironmentalProductDeclarationFromResourceDocument(document);
            }
            catch (Exception e)
            {
                BH.Engine.Base.Compute.RecordError($"Failed to map resource document to EPD. Error: {e.Message}");
                return null;
            }
        }

        [Description("Creates EnvironmentalProductDeclarations for every hit in a materials carbon search response.")]
        [Input("response", "Aggregated search response from Pull (MaterialsCarbonDataApiRequest).")]
        [Output("epds", "One EPD per hit document; entries with null results are omitted.")]
        public static List<EnvironmentalProductDeclaration> ToEnvironmentalProductDeclarations(MaterialsCarbonDataSearchResponse response)
        {
            List<EnvironmentalProductDeclaration> results = new List<EnvironmentalProductDeclaration>();
            if (response?.Hits == null)
                return results;

            foreach (MaterialsCarbonSearchHit hit in response.Hits)
            {
                if (hit?.Document == null)
                    continue;

                EnvironmentalProductDeclaration epd = ToEnvironmentalProductDeclaration(hit.Document);
                if (epd != null)
                    results.Add(epd);
            }

            return results;
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static EnvironmentalProductDeclaration EnvironmentalProductDeclarationFromResourceDocument(MaterialsCarbonResourceDocument document)
        {
            EnvironmentalProductDeclaration epd = new EnvironmentalProductDeclaration();

            MaterialsCarbonResourceNaming naming = document.Naming ?? new MaterialsCarbonResourceNaming();
            if (!string.IsNullOrWhiteSpace(naming.StaticFullName))
                epd.Name = naming.StaticFullName;
            else if (!string.IsNullOrWhiteSpace(naming.NameEN))
                epd.Name = naming.NameEN;

            epd.Type = EPDType.Product;
            epd.QuantityType = ParseQuantityTypeFromUnit(document.Physical?.UnitForData);
            epd.EnvironmentalMetrics = ParseEnvironmentalMetricsFromImpacts(document.Impacts);

            EPDDensity densityFragment = ParseDensityFromPhysical(document.Physical, epd.QuantityType);
            if (densityFragment != null)
                epd.Fragments.Add(densityFragment);

            AdditionalEPDData additionalData = ParseAdditionalEPDDataFromDocument(document);
            if (additionalData != null)
                epd.Fragments.Add(additionalData);

            return epd;
        }

        /***************************************************/

        private static EnvironmentalProductDeclaration EnvironmentalProductDeclarationFromJsonElement(JsonElement root)
        {
            EnvironmentalProductDeclaration epd = new EnvironmentalProductDeclaration();

            if (root.TryGetProperty("staticFullName", out JsonElement fullName) && fullName.ValueKind != JsonValueKind.Null)
                epd.Name = fullName.GetString();
            else if (root.TryGetProperty("nameEN", out JsonElement nameEn) && nameEn.ValueKind != JsonValueKind.Null)
                epd.Name = nameEn.GetString();

            epd.Type = EPDType.Product;
            epd.QuantityType = ParseQuantityType(root);
            epd.EnvironmentalMetrics = ParseEnvironmentalMetrics(root);

            EPDDensity densityFragment = ParseDensityFragment(root, epd.QuantityType);
            if (densityFragment != null)
                epd.Fragments.Add(densityFragment);

            AdditionalEPDData additionalData = ParseAdditionalEPDData(root);
            if (additionalData != null)
                epd.Fragments.Add(additionalData);

            return epd;
        }

        /***************************************************/

        private static QuantityType ParseQuantityTypeFromUnit(string unitForData)
        {
            if (string.IsNullOrWhiteSpace(unitForData))
                return QuantityType.Undefined;

            string unitString = unitForData.ToLowerInvariant();

            if (m_UnitToQuantityType.TryGetValue(unitString, out QuantityType qt))
                return qt;

            BH.Engine.Base.Compute.RecordWarning($"Unrecognised unit for data '{unitForData}'. QuantityType set to Undefined.");
            return QuantityType.Undefined;
        }

        /***************************************************/

        private static QuantityType ParseQuantityType(JsonElement root)
        {
            if (!root.TryGetProperty("unitForData", out JsonElement unit) || unit.ValueKind == JsonValueKind.Null)
                return QuantityType.Undefined;

            return ParseQuantityTypeFromUnit(unit.GetString());
        }

        /***************************************************/

        private static List<IEnvironmentalMetric> ParseEnvironmentalMetricsFromImpacts(Dictionary<string, MaterialsCarbonImpactModule> impacts)
        {
            List<IEnvironmentalMetric> metrics = new List<IEnvironmentalMetric>();
            if (impacts == null || impacts.Count == 0)
                return metrics;

            Dictionary<Type, Dictionary<Module, double>> accumulated = new Dictionary<Type, Dictionary<Module, double>>();

            PropertyInfo[] moduleProps = typeof(MaterialsCarbonImpactModule).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetCustomAttribute<JsonPropertyNameAttribute>() != null)
                .ToArray();

            foreach (KeyValuePair<string, MaterialsCarbonImpactModule> stage in impacts)
            {
                if (!m_StageToModule.TryGetValue(stage.Key, out Module module))
                    continue;

                MaterialsCarbonImpactModule mod = stage.Value;
                if (mod == null)
                    continue;

                foreach (PropertyInfo prop in moduleProps)
                {
                    JsonPropertyNameAttribute jn = prop.GetCustomAttribute<JsonPropertyNameAttribute>();
                    if (!m_MetricKeyToType.TryGetValue(jn.Name, out Type metricType))
                        continue;

                    object raw = prop.GetValue(mod);
                    if (!(raw is double value))
                        continue;

                    if (!accumulated.ContainsKey(metricType))
                        accumulated[metricType] = new Dictionary<Module, double>();

                    if (!accumulated[metricType].ContainsKey(module))
                        accumulated[metricType][module] = value;
                }
            }

            foreach (KeyValuePair<Type, Dictionary<Module, double>> kvp in accumulated)
            {
                try
                {
                    IEnvironmentalMetric metric = (IEnvironmentalMetric)Activator.CreateInstance(kvp.Key);
                    PropertyInfo indicatorsProperty = kvp.Key.GetProperty("Indicators");
                    indicatorsProperty.SetValue(metric, kvp.Value);
                    metrics.Add(metric);
                }
                catch (Exception e)
                {
                    BH.Engine.Base.Compute.RecordWarning($"Failed to create environmental metric of type {kvp.Key.Name}. Error: {e.Message}");
                }
            }

            return metrics;
        }

        /***************************************************/

        private static List<IEnvironmentalMetric> ParseEnvironmentalMetrics(JsonElement root)
        {
            List<IEnvironmentalMetric> metrics = new List<IEnvironmentalMetric>();

            if (!root.TryGetProperty("impacts", out JsonElement impacts) || impacts.ValueKind != JsonValueKind.Object)
                return metrics;

            Dictionary<Type, Dictionary<Module, double>> accumulated = new Dictionary<Type, Dictionary<Module, double>>();

            foreach (JsonProperty stage in impacts.EnumerateObject())
            {
                if (!m_StageToModule.TryGetValue(stage.Name, out Module module))
                    continue;

                if (stage.Value.ValueKind != JsonValueKind.Object)
                    continue;

                foreach (JsonProperty metricProp in stage.Value.EnumerateObject())
                {
                    if (!m_MetricKeyToType.TryGetValue(metricProp.Name, out Type metricType))
                        continue;

                    if (metricProp.Value.ValueKind != JsonValueKind.Number)
                        continue;

                    if (!accumulated.ContainsKey(metricType))
                        accumulated[metricType] = new Dictionary<Module, double>();

                    if (!accumulated[metricType].ContainsKey(module))
                        accumulated[metricType][module] = metricProp.Value.GetDouble();
                }
            }

            foreach (KeyValuePair<Type, Dictionary<Module, double>> kvp in accumulated)
            {
                try
                {
                    IEnvironmentalMetric metric = (IEnvironmentalMetric)Activator.CreateInstance(kvp.Key);
                    PropertyInfo indicatorsProperty = kvp.Key.GetProperty("Indicators");
                    indicatorsProperty.SetValue(metric, kvp.Value);
                    metrics.Add(metric);
                }
                catch (Exception e)
                {
                    BH.Engine.Base.Compute.RecordWarning($"Failed to create environmental metric of type {kvp.Key.Name}. Error: {e.Message}");
                }
            }

            return metrics;
        }

        /***************************************************/

        private static EPDDensity ParseDensityFromPhysical(MaterialsCarbonResourcePhysical physical, QuantityType quantityType)
        {
            double? density = physical?.Density;
            if (density.HasValue && density.Value > 0)
                return new EPDDensity { Density = density.Value };

            if (quantityType == QuantityType.Mass)
                BH.Engine.Base.Compute.RecordWarning("This EPD has a QuantityType of Mass but no density value was returned by the API. Density is required for downstream LCA calculations.");

            return null;
        }

        /***************************************************/

        private static EPDDensity ParseDensityFragment(JsonElement root, QuantityType quantityType)
        {
            bool hasDensity = root.TryGetProperty("density", out JsonElement densityEl)
                && densityEl.ValueKind == JsonValueKind.Number;

            if (hasDensity && densityEl.GetDouble() > 0)
                return new EPDDensity { Density = densityEl.GetDouble() };

            if (quantityType == QuantityType.Mass)
                BH.Engine.Base.Compute.RecordWarning("This EPD has a QuantityType of Mass but no density value was returned by the API. Density is required for downstream LCA calculations.");

            return null;
        }

        /***************************************************/

        private static AdditionalEPDData ParseAdditionalEPDDataFromDocument(MaterialsCarbonResourceDocument document)
        {
            MaterialsCarbonResourceIdentifiers ids = document.Identifiers ?? new MaterialsCarbonResourceIdentifiers();
            MaterialsCarbonResourceOrganisation org = document.Organisation ?? new MaterialsCarbonResourceOrganisation();
            MaterialsCarbonResourceDataSource source = document.DataSource ?? new MaterialsCarbonResourceDataSource();
            MaterialsCarbonResourcePhysical physical = document.Physical ?? new MaterialsCarbonResourcePhysical();

            AdditionalEPDData data = new AdditionalEPDData();
            bool hasData = false;

            if (!string.IsNullOrWhiteSpace(ids.EpdNumber))
            {
                data.Id = ids.EpdNumber;
                hasData = true;
            }

            if (!string.IsNullOrWhiteSpace(org.Manufacturer))
            {
                data.Manufacturer = org.Manufacturer;
                hasData = true;
            }

            if (!string.IsNullOrWhiteSpace(org.EpdProgram))
            {
                data.Publisher = org.EpdProgram;
                hasData = true;
            }

            if (source.EnvironmentDataPeriod.HasValue)
            {
                try
                {
                    data.ReferenceYear = (int)source.EnvironmentDataPeriod.Value;
                    hasData = true;
                }
                catch (Exception e)
                {
                    BH.Engine.Base.Compute.RecordWarning($"Failed to parse environmentDataPeriod. Error: {e.Message}");
                }
            }

            if (physical.ServiceLife.HasValue)
            {
                try
                {
                    data.LifeSpan = (int)physical.ServiceLife.Value;
                    hasData = true;
                }
                catch (Exception e)
                {
                    BH.Engine.Base.Compute.RecordWarning($"Failed to parse serviceLife. Error: {e.Message}");
                }
            }

            if (!string.IsNullOrWhiteSpace(source.EnvironmentDataSourceStandard))
            {
                data.IndustryStandards = new List<string> { source.EnvironmentDataSourceStandard };
                hasData = true;
            }

            if (!string.IsNullOrWhiteSpace(org.ProductDescription))
            {
                data.Description = org.ProductDescription;
                hasData = true;
            }

            return hasData ? data : null;
        }

        /***************************************************/

        private static AdditionalEPDData ParseAdditionalEPDData(JsonElement root)
        {
            AdditionalEPDData data = new AdditionalEPDData();
            bool hasData = false;

            if (root.TryGetProperty("epdNumber", out JsonElement epdNum) && epdNum.ValueKind != JsonValueKind.Null)
            {
                data.Id = epdNum.GetString();
                hasData = true;
            }

            if (root.TryGetProperty("manufacturer", out JsonElement manufacturer) && manufacturer.ValueKind != JsonValueKind.Null)
            {
                data.Manufacturer = manufacturer.GetString();
                hasData = true;
            }

            if (root.TryGetProperty("epdProgram", out JsonElement epdProgram) && epdProgram.ValueKind != JsonValueKind.Null)
            {
                data.Publisher = epdProgram.GetString();
                hasData = true;
            }

            if (root.TryGetProperty("environmentDataPeriod", out JsonElement dataPeriod) && dataPeriod.ValueKind != JsonValueKind.Null)
            {
                try
                {
                    if (dataPeriod.ValueKind == JsonValueKind.Number)
                    {
                        double d = dataPeriod.GetDouble();
                        data.ReferenceYear = (int)d;
                        hasData = true;
                    }
                    else if (dataPeriod.ValueKind == JsonValueKind.String)
                    {
                        string s = dataPeriod.GetString();
                        if (!string.IsNullOrWhiteSpace(s))
                        {
                            if (int.TryParse(s, out int parsed))
                            {
                                data.ReferenceYear = parsed;
                                hasData = true;
                            }
                            else if (double.TryParse(s, out double d2))
                            {
                                data.ReferenceYear = (int)d2;
                                hasData = true;
                            }
                            else
                            {
                                BH.Engine.Base.Compute.RecordWarning($"Unrecognised format for environmentDataPeriod: '{s}'");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    BH.Engine.Base.Compute.RecordWarning($"Failed to parse environmentDataPeriod. Error: {e.Message}");
                }
            }

            if (root.TryGetProperty("serviceLife", out JsonElement serviceLife) && serviceLife.ValueKind != JsonValueKind.Null)
            {
                try
                {
                    if (serviceLife.ValueKind == JsonValueKind.Number)
                    {
                        double d = serviceLife.GetDouble();
                        data.LifeSpan = (int)d;
                        hasData = true;
                    }
                    else if (serviceLife.ValueKind == JsonValueKind.String)
                    {
                        string s = serviceLife.GetString();
                        if (!string.IsNullOrWhiteSpace(s))
                        {
                            if (int.TryParse(s, out int parsed))
                            {
                                data.LifeSpan = parsed;
                                hasData = true;
                            }
                            else if (double.TryParse(s, out double d2))
                            {
                                data.LifeSpan = (int)d2;
                                hasData = true;
                            }
                            else
                            {
                                BH.Engine.Base.Compute.RecordWarning($"Unrecognised format for serviceLife: '{s}'");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    BH.Engine.Base.Compute.RecordWarning($"Failed to parse serviceLife. Error: {e.Message}");
                }
            }

            if (root.TryGetProperty("environmentDataSourceStandard", out JsonElement dataStandard) && dataStandard.ValueKind != JsonValueKind.Null)
            {
                string standard = dataStandard.GetString();
                if (!string.IsNullOrEmpty(standard))
                {
                    data.IndustryStandards = new List<string> { standard };
                    hasData = true;
                }
            }

            if (root.TryGetProperty("productDescription", out JsonElement description) && description.ValueKind != JsonValueKind.Null)
            {
                data.Description = description.GetString();
                hasData = true;
            }

            return hasData ? data : null;
        }


        /***************************************************/
        /**** Private Static Fields                     ****/
        /***************************************************/

        private static readonly Dictionary<string, Module> m_StageToModule = new Dictionary<string, Module>
        {
            { "A1",    Module.A1 },
            { "A2",    Module.A2 },
            { "A3",    Module.A3 },
            { "A1-A3", Module.A1toA3 },
            { "A4",    Module.A4 },
            { "A5",    Module.A5 },
            { "B1",    Module.B1 },
            { "B2",    Module.B2 },
            { "B3",    Module.B3 },
            { "B4",    Module.B4 },
            { "B5",    Module.B5 },
            { "B6",    Module.B6 },
            { "B7",    Module.B7 },
            { "C1",    Module.C1 },
            { "C2",    Module.C2 },
            { "C3",    Module.C3 },
            { "C4",    Module.C4 },
            { "C1-C4", Module.C1toC4 },
            { "D",     Module.D }
        };

        /***************************************************/

        private static readonly Dictionary<string, Type> m_MetricKeyToType = new Dictionary<string, Type>
        {
            { "impactGWP100_kgCO2e",          typeof(ClimateChangeTotalNoBiogenicMetric) },
            { "impactGWP100_kgCO2e_total",    typeof(ClimateChangeTotalMetric) },
            { "impactGWP100_kgCO2e_fossil",   typeof(ClimateChangeFossilMetric) },
            { "impactGWP100_kgCO2e_biogenic", typeof(ClimateChangeBiogenicMetric) },
            { "impactGWP100_kgCO2e_luluc",    typeof(ClimateChangeLandUseMetric) },
            { "traciGWP_kgCO2e",              typeof(ClimateChangeTotalNoBiogenicMetric) }
        };

        /***************************************************/

        private static readonly Dictionary<string, QuantityType> m_UnitToQuantityType = new Dictionary<string, QuantityType>
        {
            { "kg",   QuantityType.Mass },
            { "ton",  QuantityType.Mass },
            { "lbs",  QuantityType.Mass },
            { "m2",   QuantityType.Area },
            { "sqft", QuantityType.Area },
            { "m3",   QuantityType.Volume },
            { "cuft", QuantityType.Volume },
            { "m",    QuantityType.Length },
            { "ft",   QuantityType.Length },
            { "item", QuantityType.Item },
            { "unit", QuantityType.Item }
        };

        /***************************************************/
    }
}
