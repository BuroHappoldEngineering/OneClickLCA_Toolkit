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
using System.Collections.Generic;
using System.ComponentModel;

namespace BH.Engine.Adapters.OneClickLCA
{
    public static partial class Compute
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Maps calculation-results dictionary payload to a lookup of tool id to rule and category display name maps.")]
        [Input("dictionaryData", "Dictionary data from GET /calculation-results/dictionary (DictionaryResponse.DictionaryData).")]
        [Output("mappingsByToolId", "Tool id to DesignToolDictionaryMapping; empty if dictionaryData is null or has no tool entries. Duplicate tool ids overwrite earlier entries with a warning.")]
        public static Dictionary<string, DesignToolDictionaryMapping> MapDictionaryDataByToolId(DictionaryData dictionaryData)
        {
            Dictionary<string, DesignToolDictionaryMapping> results = new Dictionary<string, DesignToolDictionaryMapping>();

            if (dictionaryData == null)
            {
                BH.Engine.Base.Compute.RecordError("DictionaryData is null.");
                return results;
            }

            string designId = dictionaryData.DesignId ?? "";

            if (dictionaryData.ToolData == null)
                return results;

            foreach (ToolData toolData in dictionaryData.ToolData)
            {
                if (toolData == null)
                    continue;

                if (toolData.Tool == null || toolData.Tool.Count == 0)
                {
                    BH.Engine.Base.Compute.RecordWarning("Skipped a toolData entry with no tool identifier in the tool map.");
                    continue;
                }

                if (toolData.Tool.Count > 1)
                {
                    BH.Engine.Base.Compute.RecordWarning(
                        "toolData contains multiple tool keys; producing one DesignToolDictionaryMapping per key with the same rules and categories.");
                }

                Dictionary<string, string> rules = toolData.Rules != null
                    ? new Dictionary<string, string>(toolData.Rules)
                    : new Dictionary<string, string>();

                Dictionary<string, string> categories = toolData.Categories != null
                    ? new Dictionary<string, string>(toolData.Categories)
                    : new Dictionary<string, string>();

                foreach (KeyValuePair<string, string> toolEntry in toolData.Tool)
                {
                    string toolId = toolEntry.Key ?? "";

                    DesignToolDictionaryMapping mapping = new DesignToolDictionaryMapping
                    {
                        DesignId = designId,
                        ToolId = toolId,
                        ToolDisplayName = toolEntry.Value ?? "",
                        Rules = new Dictionary<string, string>(rules),
                        Categories = new Dictionary<string, string>(categories)
                    };

                    if (results.ContainsKey(toolId))
                    {
                        BH.Engine.Base.Compute.RecordWarning(
                            $"Duplicate tool id '{toolId}' in dictionary data; the later entry replaces the earlier one.");
                    }

                    results[toolId] = mapping;
                }
            }

            return results;
        }

        /***************************************************/
    }
}
