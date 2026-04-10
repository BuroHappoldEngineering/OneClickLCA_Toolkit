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

using BH.oM.Base;
using BH.oM.Base.Attributes;
using BH.oM.Data.Requests;
using System.ComponentModel;

namespace BH.oM.Adapters.OneClickLCA
{
    [Description("Request resource search from the OneClick LCA Materials Carbon Data API. Pull returns a single MaterialsCarbonDataSearchResponse (hits aggregated across internal pagination). Use Engine Compute ToEnvironmentalProductDeclaration / ToEnvironmentalProductDeclarations to obtain EnvironmentalProductDeclaration objects.")]
    public class MaterialsCarbonDataApiRequest : BHoMObject, IRequest
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("OAuth2 client identifier used for client credentials authentication.")]
        public virtual string ClientId { get; set; } = "";

        [Description("OAuth2 client secret used for client credentials authentication.")]
        public virtual string ClientSecret { get; set; } = "";

        [Description("The query text to search for. Use * to return all resources matching the filter, or provide a specific term such as a material name or resource ID. Defaults to * (all resources).")]
        public virtual string SearchQuery { get; set; } = "*";

        [Description("One or more field names to search against, comma-separated. For example: searchString, resourceId, manufacturer. May be omitted when SearchQuery is *.")]
        public virtual string QueryBy { get; set; } = "";

        [Description("Filter expression to refine results. For example: areas:UnitedKingdom or dataProperties:=CML. Multiple conditions can be combined with && or ||.")]
        public virtual string FilterBy { get; set; } = "";

        [Description("Sort expression for ordering results. For example: firstUploadTime:desc or manufacturer:asc. Multiple fields can be comma-separated.")]
        public virtual string SortBy { get; set; } = "";

        [Description("Maximum total number of search hits to return. The adapter paginates internally using pages of up to 250. Defaults to 250.")]
        public virtual int MaxResults { get; set; } = 250;

        /***************************************************/
    }
}




