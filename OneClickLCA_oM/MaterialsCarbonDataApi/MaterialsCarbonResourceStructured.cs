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
using System.Collections.Generic;
using System.ComponentModel;

namespace BH.oM.Adapters.OneClickLCA
{
    [Description("Structured, grouped view of a materials carbon resource document. Mirrors MaterialsCarbonResourceDocument after Pull; assign child references from the document or build in Engine.")]
    public class MaterialsCarbonResourceStructured : BHoMObject
    {
        [Description("Identifiers and resource typing.")]
        public virtual MaterialsCarbonResourceIdentifiers Identifiers { get; set; } = new MaterialsCarbonResourceIdentifiers();

        [Description("Names in several languages.")]
        public virtual MaterialsCarbonResourceNaming Naming { get; set; } = new MaterialsCarbonResourceNaming();

        [Description("Data source and programme metadata.")]
        public virtual MaterialsCarbonResourceDataSource DataSource { get; set; } = new MaterialsCarbonResourceDataSource();

        [Description("Declared unit and physical properties.")]
        public virtual MaterialsCarbonResourcePhysical Physical { get; set; } = new MaterialsCarbonResourcePhysical();

        [Description("Organisation, verification, and lists.")]
        public virtual MaterialsCarbonResourceOrganisation Organisation { get; set; } = new MaterialsCarbonResourceOrganisation();

        [Description("Per-module impacts (same entries as on the source document; shared module references when mapped from one document).")]
        public virtual Dictionary<string, MaterialsCarbonImpactModule> Impacts { get; set; } = new Dictionary<string, MaterialsCarbonImpactModule>();
    }
}
