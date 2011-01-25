// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Contains information that identifies a particular file. It is used by IColumnProvider::GetItemData when requesting data for a particular file (SHCOLUMNDATA).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct ShellColumnData
    {
        /// <summary>
        /// combination of SHCDF_ flags (dwFlags).
        /// </summary>
        public int Flags;

        /// <summary>
        /// File attributes (dwFileAttributes).
        /// </summary>
        public int FileAttributes;

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        private readonly int Reserved;

        /// <summary>
        /// Address of file name extension.
        /// </summary>
        public IntPtr FileNameExtension;

        /// <summary>
        /// Absolute path of file (wszFile[MAX_PATH]).
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string File;
    }
}
