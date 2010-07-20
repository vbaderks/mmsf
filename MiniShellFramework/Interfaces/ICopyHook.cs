// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace MiniShellFramework.Interfaces
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
    public interface ICopyHook
    {
        [PreserveSig]
        uint CopyCallback(IntPtr hwnd, FileOperation fileOperation, uint flags, string sourceFile, uint sourceAttributes,
                          string destinationFile, uint destinationAttributes);
    }
}
