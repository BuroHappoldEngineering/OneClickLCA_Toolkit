using BH.oM.Python;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.Adapters.OneClickLCA
{
    public static partial class Compute
    {
        //TODO: write python env install method.
        public static PythonEnvironment InstallPythonEnv_OneCliclLCA(bool run = false, bool reinstall = false)
        {
            if (!run)
                return null;

            if (!reinstall)
                return Python.Compute.VirtualEnvironment(oM.Python.Enums.PythonVersion.v3_13, Query.ToolkitName());

            PythonEnvironment virtualEnvironment = Python.Compute.VirtualEnvironment(oM.Python.Enums.PythonVersion.v3_13, Query.ToolkitName(), false);

            Python.Compute.InstallPackageLocal(virtualEnvironment, System.IO.Path.Combine(Python.Query.DirectoryCode(), Python.Query.ToolkitName()));
            Python.Compute.InstallPackageLocal(virtualEnvironment, System.IO.Path.Combine(Python.Query.DirectoryCode(), Query.ToolkitName()));

            return virtualEnvironment;
        }
    }
}
