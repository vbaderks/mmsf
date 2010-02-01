// <copyright company="Victor Derks">
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace MiniShellFramework.Interfaces
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("0000010c-0000-0000-C000-000000000046")]
    public interface ICopyHook
    {
        void CopyCallback(IntPtr hwnd, FileOperation fileOperation, uint flags, string sourceFile, uint sourceAttributes,
                          string destinationFile, uint destinationAttributes);
    }
}
