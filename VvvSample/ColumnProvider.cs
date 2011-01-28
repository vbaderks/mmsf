// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using MiniShellFramework;

namespace VvvSample
{
    [ComVisible(true)]                              // Make this .NET class a COM object (ComVisible is false on assembly level).
    [Guid("FC2347F2-743F-46A7-B205-324140030BF3")]  // Explicitly assign a GUID: easier to reference and to debug.
    [ClassInterface(ClassInterfaceType.None)]       // Only the functions from the COM interfaces should be accessible.
    public sealed class ColumnProvider : ColumnProviderBase
    {
        [ComRegisterFunction]
        public static void ComRegisterFunction(Type type)
        {
            Contract.Requires(type != null);

            ComRegister(type, "MMSF Sample ShellExtension (ColumnProvider)");
        }

        [ComUnregisterFunction]
        public static void ComUnregisterFunction(Type type)
        {
            Contract.Requires(type != null);

            ComUnregister(type);
        }

        protected override void InitializeCore(string folderName)
        {
        //// Register the supported columns.
        //RegisterColumn(IDS_SHELLEXT_LABEL, 9);
        //RegisterColumn(IDS_SHELLEXT_FILECOUNT, 14, LVCFMT_RIGHT, SHCOLSTATE_TYPE_INT);

        //GUID guidSummaryInformation = PSGUID_SUMMARYINFORMATION;
        //RegisterColumn(guidSummaryInformation, PID_AUTHOR, L"Author", 
        //    14, LVCFMT_RIGHT, SHCOLSTATE_TYPE_STR | SHCOLSTATE_SLOW);

        //RegisterExtension(wszVVVExtension);
        //// .. this would be the place to add other extensions this columnprovider also supports.
        }
    }
}
