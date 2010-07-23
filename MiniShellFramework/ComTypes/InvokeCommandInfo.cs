// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Contains information needed by IContextMenu::InvokeCommand to 
    /// invoke a shortcut menu command (CMINVOKECOMMANDINFO).
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct InvokeCommandInfo
    {
        /// <summary>
        /// The size of this structure, in bytes (cbSize).
        /// </summary>
        public int Size;           // sizeof(CMINVOKECOMMANDINFO)

        /// <summary>
        /// Any combination of CMIC_MASK_*.
        /// </summary>
        public int fMask;

        /// <summary>
        /// Might be NULL, indicating no owner window (hwnd).
        /// </summary>
        public IntPtr parentWindow;

        /// <summary>
        /// either a string or MAKEINTRESOURCE(idOffset).
        /// </summary>
        public IntPtr lpVerb;

        /// <summary>
        /// might be NULL (indicating no parameter).
        /// </summary>
        public string lpParameters;

        /// <summary>
        /// Might be NULL (indicating no specific directory).
        /// </summary>
        public string lpDirectory;

        /// <summary>
        /// one of SW_ values for ShowWindow() API.
        /// </summary>
        public int nShow;

        /// <summary>
        /// An optional keyboard shortcut to assign to any application activated by the command.
        /// If the fMask parameter does not specify CMIC_MASK_HOTKEY, this member is ignored.
        /// </summary>
        public int dwHotKey;

        /// <summary>
        /// An icon to use for any application activated by the command.
        /// If the fMask member does not specify CMIC_MASK_ICON, this member is ignored.
        /// </summary>
        public IntPtr hIcon;
    }
}
