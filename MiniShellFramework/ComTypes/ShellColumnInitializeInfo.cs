// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System.Runtime.InteropServices;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Passes initialization information to IColumnProvider::Initialize (SHCOLUMNINIT).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct ShellColumnInitializeInfo
    {
        /// <summary>
        /// Initialization flags. Reserved. Set to 0 (dwFlags).
        /// </summary>
        private readonly int Flags;

        /// <summary>
        /// Reserved for future use. Set to NULL (dwReserved).
        /// </summary>
        private readonly int Reserved;

        /// <summary>
        /// Fully qualified folder path. Empty if multiple folders are specified (wszFolder[MAX_PATH]).
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string Folder;
    }
}
