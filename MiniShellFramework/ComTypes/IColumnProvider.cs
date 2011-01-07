// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MiniShellFramework.ComTypes
{
    struct SHCOLUMNINIT
    {
        public int Flags;         //     ULONG   dwFlags;              // initialization flags
        public int Reserved;      // ULONG   dwReserved;           // reserved for future use.
        public string Folder;     // WCHAR   wszFolder[MAX_PATH];  // fully qualified folder path (or empty if multiple folders)
    }

    /// <summary>
    /// This is the definition of the COM shell interface ICopyHook.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214FC-0000-0000-c000-000000000046")] // TODO
    //[ContractClass(typeof(CopyHookContract))]
    public interface IColumnProvider
    {
    }
}
