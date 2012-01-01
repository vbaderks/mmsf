// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Provides means by which to initialize a shell extension with a ShellObject
    /// </summary>
    [ComImport]                                           // Mark that this interface is already defined in a standard COM typelib. Prevent .NET from registering a typelib for it.
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)] // Mark that this interface directly derived from IUnknown in the COM world.
    [Guid("7f73be3f-fb79-493c-a6c7-7ee14e245841")]
    public interface IInitializeWithItem
    {
        /// <summary>
        /// Initializes with ShellItem
        /// </summary>
        /// <param name="shellItem">A reference to a ShellItem.</param>
        /// <param name="storageMode">The storage mode.</param>
        void Initialize(IntPtr shellItem, StorageModes storageMode);
    }
}
