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

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BH.oM.Adapters.OneClickLCA
{
    public sealed class MaterialsCarbonResourceDocumentJsonConverter : JsonConverter<MaterialsCarbonResourceDocument>
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public override MaterialsCarbonResourceDocument Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Expected start of object for materials carbon resource document.");

            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = doc.RootElement;
                string fullJson = root.GetRawText();

                MaterialsCarbonResourceDocument result = new MaterialsCarbonResourceDocument
                {
                    Identifiers = JsonSerializer.Deserialize<MaterialsCarbonResourceIdentifiers>(fullJson, options)
                        ?? new MaterialsCarbonResourceIdentifiers(),
                    Naming = JsonSerializer.Deserialize<MaterialsCarbonResourceNaming>(fullJson, options)
                        ?? new MaterialsCarbonResourceNaming(),
                    DataSource = JsonSerializer.Deserialize<MaterialsCarbonResourceDataSource>(fullJson, options)
                        ?? new MaterialsCarbonResourceDataSource(),
                    Physical = JsonSerializer.Deserialize<MaterialsCarbonResourcePhysical>(fullJson, options)
                        ?? new MaterialsCarbonResourcePhysical(),
                    Organisation = JsonSerializer.Deserialize<MaterialsCarbonResourceOrganisation>(fullJson, options)
                        ?? new MaterialsCarbonResourceOrganisation(),
                    DeclaredImpacts = JsonSerializer.Deserialize<MaterialsCarbonResourceDeclaredImpacts>(fullJson, options)
                        ?? new MaterialsCarbonResourceDeclaredImpacts()
                };

                if (root.TryGetProperty("impacts", out JsonElement impactsEl) && impactsEl.ValueKind == JsonValueKind.Object)
                {
                    result.Impacts = JsonSerializer.Deserialize<Dictionary<string, MaterialsCarbonImpactModule>>(
                            impactsEl.GetRawText(),
                            options)
                        ?? new Dictionary<string, MaterialsCarbonImpactModule>();
                }
                else
                {
                    result.Impacts = new Dictionary<string, MaterialsCarbonImpactModule>();
                }

                return result;
            }
        }

        public override void Write(Utf8JsonWriter writer, MaterialsCarbonResourceDocument value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartObject();

            WriteMergedObject(writer, JsonSerializer.SerializeToElement(value.Identifiers, options));
            WriteMergedObject(writer, JsonSerializer.SerializeToElement(value.Naming, options));
            WriteMergedObject(writer, JsonSerializer.SerializeToElement(value.DataSource, options));
            WriteMergedObject(writer, JsonSerializer.SerializeToElement(value.Physical, options));
            WriteMergedObject(writer, JsonSerializer.SerializeToElement(value.Organisation, options));
            WriteMergedObject(writer, JsonSerializer.SerializeToElement(value.DeclaredImpacts, options));

            if (value.Impacts != null && value.Impacts.Count > 0)
            {
                writer.WritePropertyName("impacts");
                JsonSerializer.Serialize(writer, value.Impacts, options);
            }

            writer.WriteEndObject();
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static void WriteMergedObject(Utf8JsonWriter writer, JsonElement element)
        {
            if (element.ValueKind != JsonValueKind.Object)
                return;

            foreach (JsonProperty property in element.EnumerateObject())
                property.WriteTo(writer);
        }

        /***************************************************/
    }
}
