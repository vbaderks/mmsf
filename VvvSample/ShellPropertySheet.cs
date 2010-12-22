// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using MiniShellFramework;

namespace VvvSample
{
    [ComVisible(true)]                              // Make this .NET class a COM object (ComVisible is false on assembly level).
    [Guid("8CB563CC-24D1-464D-847F-4A215F2487A9")]  // Explicitly assign a GUID: easier to reference and to debug.
    [ClassInterface(ClassInterfaceType.None)]       // Only the functions from the COM interfaces should be accessible.
    public sealed class ShellPropertySheet : ShellPropertySheetBase
    {
        private const string registryDescription = "VVV ShellPropertySheet (MMSF Sample)";

        [ComRegisterFunction]
        public static void ComRegisterFunction(Type type)
        {
            Contract.Requires(type != null);

            VvvRootKey.Register();
            ComRegister(type, registryDescription, VvvRootKey.ProgId);
        }

        [ComUnregisterFunction]
        public static void ComUnregisterFunction(Type type)
        {
            Contract.Requires(type != null);

            ComUnregister(type, registryDescription, VvvRootKey.ProgId);
            VvvRootKey.Unregister();
        }

        public ShellPropertySheet()
        {
            RegisterExtension(".mvvv");
        }

        protected override void OnAddPages(IList<string> fileNames)
        {
            // Only add the page if only 1 file is selected and is of our own extension.
            if (fileNames.Count != 1 || ContainsUnknownExtension(fileNames))
                return; // In this sample only extend the context sheet when only 1 .mvvv file is selected

            ////addpage(CPropertyPageVVV::CreateInstance(filenames[0]));
        }
    }
}
