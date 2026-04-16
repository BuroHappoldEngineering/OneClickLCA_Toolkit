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

using BH.oM.Adapters.OneClickLCA;
using BH.oM.Base;
using BH.oM.Base.Attributes;
using BH.oM.LifeCycleAssessment;
using BH.oM.LifeCycleAssessment.MaterialFragments;
using BH.oM.LifeCycleAssessment.Results;
using BH.oM.OneClickLCA.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BH.Engine.Adapters.OneClickLCA
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Gets the total for each environmental metric, LCA module and RICS category from the provided report.")]
        [Input("report", "OneClick LCA report.")]
        [Input("categoryLevel", "category level to calculate the totals over. If left at 0, the totals will be calculated for all categories found in the input regardless of their level.")]
        [Output("totals", "Total for each environmental metric, LCA module and RICS category")]
        public static Dictionary<RICSCategory, List<MaterialResult>> TotalPerCategory(this OneClickReport report, int categoryLevel = 0)
        {
            if (report == null)
                return new Dictionary<RICSCategory, List<MaterialResult>>();
            else
                return report.Entries.TotalPerCategory(categoryLevel);
        }

        /***************************************************/

        [Description("Gets the total for each environmental metric, LCA module and RICS category from the provided set of report entries.")]
        [Input("entries", "OneClick LCA report entries.")]
        [Input("categoryLevel", "category level to calculate the totals over. If left at 0, the totals will be calculated for all categories found in the input regardless of their level.")]
        [Output("totals", "Total for each environmental metric, LCA module and RICS category")]
        public static Dictionary<RICSCategory, List<MaterialResult>> TotalPerCategory(this List<ReportEntry> entries, int categoryLevel = 0)
        {
            IEnumerable<IGrouping<RICSCategory, ReportEntry>> groups;
            if (categoryLevel > 0)
                groups = entries.GroupBy(x => x.RICSCategory.CategoryForLevel(categoryLevel));
            else
                groups = entries.GroupBy(x => x.RICSCategory);

            return groups.ToDictionary(
                group => group.Key, 
                group => group.SelectMany(entry => entry.EnvironmentalMetrics)
                              .GroupBy(x => x.GetType())
                              .Select(x => GetTotal(x, x.Key))
                              .ToList()
            );
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static MaterialResult GetTotal(this IEnumerable<MaterialResult> metrics, Type type)
        {
            Dictionary<Module, double> totals = new Dictionary<Module, double>();

            foreach (MaterialResult metric in metrics)
            {
                foreach (KeyValuePair<Module, double> kvp in metric.Indicators)
                {
                    if (double.IsNaN(kvp.Value))
                        continue;

                    if (totals.ContainsKey(kvp.Key))
                        totals[kvp.Key] += kvp.Value;
                    else
                        totals[kvp.Key] = kvp.Value;
                }
            }

            return (MaterialResult)Activator.CreateInstance(type, "", "", totals);
        }

        /***************************************************/

    }
}


