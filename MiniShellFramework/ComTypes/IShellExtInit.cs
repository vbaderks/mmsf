// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// The IShellExtInit interface is used by the shell to initialize shell extension objects.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("b824b49d-22ac-4161-ac8a-9916e8fa3f7f")]
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
