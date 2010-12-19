// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// The IShellExtInit interface is used by the shell to initialize shell extension objects.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214E8-0000-0000-c000-000000000046")]
    [ContractClass(typeof(ShellExtInitContract))]
    public interface IShellExtInit
    {
        /// <summary>
        /// The Initialize method is called when File Explorer is initializing a context menu extension,
        /// a property sheet extension, or a non-default drag-and-drop extension.
        /// </summary>
        /// <param name="pidlFolder">The pidl folder.</param>
        /// <param name="dataObject">The data object.</param>
        /// <param name="hkeyProgId">The hkey prog id.</param>
        void Initialize(IntPtr pidlFolder, [In] IDataObject dataObject, uint hkeyProgId);
    }
}
