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
    [JsonConverter(typeof(MaterialsCarbonResourceDocumentJsonConverter))]
    [Description("Resource document from the OneClick LCA materials carbon data search API (nested under each hit). Flat JSON is deserialized into grouped properties.")]
    public class MaterialsCarbonResourceDocument : BHoMObject
    {
        [Description("Identifiers and resource typing.")]
        public virtual MaterialsCarbonResourceIdentifiers Identifiers { get; set; } = new MaterialsCarbonResourceIdentifiers();

        [Description("Names in several languages.")]
        public virtual MaterialsCarbonResourceNaming Naming { get; set; } = new MaterialsCarbonResourceNaming();

        [Description("Data source and programme metadata.")]
        public virtual MaterialsCarbonResourceDataSource DataSource { get; set; } = new MaterialsCarbonResourceDataSource();

        [Description("Declared unit and physical properties.")]
        public virtual MaterialsCarbonResourcePhysical Physical { get; set; } = new MaterialsCarbonResourcePhysical();

        [Description("Organisation, verification, classification lists, and product text.")]
        public virtual MaterialsCarbonResourceOrganisation Organisation { get; set; } = new MaterialsCarbonResourceOrganisation();

        [Description("Per-module impact indicators (dynamic module keys such as A1-A3).")]
        public virtual Dictionary<string, MaterialsCarbonImpactModule> Impacts { get; set; } = new Dictionary<string, MaterialsCarbonImpactModule>();

        [Description("Declared-unit GWP, TRACI, and biogenic storage scalars from the document payload.")]
        public virtual MaterialsCarbonResourceDeclaredImpacts DeclaredImpacts { get; set; } = new MaterialsCarbonResourceDeclaredImpacts();
    }
}
