using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MiniShellFramework.Interfaces
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
