// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// This is the definition of the COM shell interface ICopyHook.
    /// </summary>
    /// <remarks>
    /// There are actually 2 ICopyHook COM interfaces: ICopyHookA and ICopyHookW.
    /// The shell on Windows XP and beter will first query ICopyHookW. When the extension doesn't provide that
    /// interface it will query for ICopyHookA. The used guid is that of ICopyHookW.
    /// </remarks>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214FC-0000-0000-c000-000000000046")]
    [ContractClass(typeof(CopyHookContract))]
    public interface ICopyHook
    {
        /// <summary>
        /// Callback called by the Shell. Determines whether the Shell will be allowed to move, copy, delete, or rename a folder or printer object.
        /// </summary>
        /// <param name="parentWindow">A handle to the window that the copy hook handler should use as the parent for any user interface elements the handler may need to display.</param>
        /// <param name="fileOperation">The file operation that will be performed.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="source">The source folder or printer.</param>
        /// <param name="sourceAttributes">The source attributes.</param>
        /// <param name="destination">The destination folder or printer.</param>
        /// <param name="destinationAttributes">The destination attributes.</param>
        /// <returns>
        /// IDYES: to allow the operation.
        /// IDNO: Prevents the operation on this folder but continues with any other operations that have been approved.
        /// IDCANCEL: Prevents the current operation and cancels any pending operations.
        /// </returns>
        [PreserveSig]
        uint CopyCallback(
                          IntPtr parentWindow,
                          FileOperation fileOperation,
                          uint flags,
                          [In, MarshalAs(UnmanagedType.LPWStr)] string source,
                          uint sourceAttributes,
                          [In, MarshalAs(UnmanagedType.LPWStr)] string destination,
                          uint destinationAttributes);
    }
}
