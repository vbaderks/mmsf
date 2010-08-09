// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace VvvSample
{
    [ComVisible(true)]                              // Make this .NET class a COM object (ComVisible is false on assembly level).
    [Guid("8CB563CC-24D1-464D-847F-4A215F2487A9")]  // Explicitly assign a GUID: easier to reference and to debug.
    [ClassInterface(ClassInterfaceType.None)]       // Only the functions from the COM interfaces should be accessible.
    public class ShellPropertySheet
    {
        [ComRegisterFunction]
        public static void ComRegisterFunction(Type type)
        {
            Contract.Requires(type != null);
            ////ComRegister(type, RegistryName, "MMSF Sample ShellExtension (ShellPropertySheet)");
        }

        [ComUnregisterFunction]
        public static void ComUnregisterFunction(Type type)
        {
            Contract.Requires(type != null);
            ////ComUnregister(type, RegistryName);
        }

        ShellPropertySheet()
        {
            ////RegisterExtension(tszVVVExtension);
        }

        void OnAddPages(/*const CAddPage& addpage, const vector<CString>& filenames */)
        {
            // Only add the page if only 1 file is selected and is of our own extension.
            ////if (filenames.size() != 1 || ContainsUnknownExtension(filenames))
            ////    return;

            ////addpage(CPropertyPageVVV::CreateInstance(filenames[0]));
        }
    }
}
