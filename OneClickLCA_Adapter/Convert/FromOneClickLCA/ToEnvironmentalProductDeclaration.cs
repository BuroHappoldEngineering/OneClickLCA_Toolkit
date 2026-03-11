/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
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

using BH.oM.LifeCycleAssessment;
using BH.oM.LifeCycleAssessment.Fragments;
using BH.oM.LifeCycleAssessment.MaterialFragments;
using Module = BH.oM.LifeCycleAssessment.Module;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;

namespace BH.Adapter.OneClickLCA
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static EnvironmentalProductDeclaration ToEnvironmentalProductDeclaration(string json)
        {
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
                return ToEnvironmentalProductDeclaration(doc.RootElement);
            }
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static EnvironmentalProductDeclaration ToEnvironmentalProductDeclaration(JsonElement root)
        {
            EnvironmentalProductDeclaration epd = new EnvironmentalProductDeclaration();

            // Name — prefer the full descriptive name, fall back to English name
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

        private static QuantityType ParseQuantityType(JsonElement root)
        {
            if (!root.TryGetProperty("unitForData", out JsonElement unit) || unit.ValueKind == JsonValueKind.Null)
                return QuantityType.Undefined;

            string unitString = unit.GetString()?.ToLowerInvariant();

            if (m_UnitToQuantityType.TryGetValue(unitString, out QuantityType qt))
                return qt;

            BH.Engine.Base.Compute.RecordWarning($"Unrecognised unit for data '{unit.GetString()}'. QuantityType set to Undefined.");
            return QuantityType.Undefined;
        }

        /***************************************************/

        private static List<IEnvironmentalMetric> ParseEnvironmentalMetrics(JsonElement root)
        {
            List<IEnvironmentalMetric> metrics = new List<IEnvironmentalMetric>();

            if (!root.TryGetProperty("impacts", out JsonElement impacts) || impacts.ValueKind != JsonValueKind.Object)
                return metrics;

            // Collect indicator values grouped by metric type then by module; first value wins per module
            // when multiple API keys map to the same metric type (e.g. impactGWP100_kgCO2e and traciGWP_kgCO2e).
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

            // environmentDataPeriod may be a number or a string; parse safely
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

            // serviceLife may be a number or a string; parse safely
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
            { "item", QuantityType.Item }
        };

        /***************************************************/
    }
}


