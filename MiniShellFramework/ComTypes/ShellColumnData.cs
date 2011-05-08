// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System.IO;
using System.Runtime.InteropServices;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Options used to specify the nature of the request of a ShellColumnData.
    /// </summary>
    public enum ShellColumnDataOptions
    {
        /// <summary>
        /// Flag to indicate that all cached data should be flushed (SHCDF_UPDATEITEM)
        /// </summary>
        UpdateItem = 1
    }

    /// <summary>
    /// Contains information that identifies a particular file. It is used by IColumnProvider::GetItemData when requesting data for a particular file (SHCOLUMNDATA).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct ShellColumnData
    {
        /// <summary>
        /// combination of SHCDF_ flags (dwFlags).
        /// </summary>
        public ShellColumnDataOptions Flags;

        /// <summary>
        /// File attributes (dwFileAttributes).
        /// </summary>
        public FileAttributes FileAttributes;

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        private readonly int Reserved;

        /// <summary>
        /// Address of file name extension.
        /// </summary>
        public string FileNameExtension;

        /// <summary>
        /// Absolute path of file (wszFile[MAX_PATH]).
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string File;
    }
}
