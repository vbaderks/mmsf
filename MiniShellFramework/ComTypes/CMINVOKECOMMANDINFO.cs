// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace MiniShellFramework.ComTypes
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CMINVOKECOMMANDINFO
    {
        int cbSize;          // sizeof(CMINVOKECOMMANDINFO)
        int fMask;           // any combination of CMIC_MASK_*
        IntPtr hwnd;         // might be NULL (indicating no owner window)
        string lpVerb;       // either a string or MAKEINTRESOURCE(idOffset)
        string lpParameters; // might be NULL (indicating no parameter)
        string lpDirectory;  // might be NULL (indicating no specific directory)
        int nShow;           // one of SW_ values for ShowWindow() API
        int dwHotKey;
        IntPtr hIcon;
    }
}
