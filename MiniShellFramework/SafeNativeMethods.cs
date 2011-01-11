// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using MiniShellFramework.ComTypes;

namespace MiniShellFramework
{
    internal static class SafeNativeMethods
    {
        /// <summary>
        /// Retrieves the names of dropped files that result from a successful drag-and-drop operation.
        /// </summary>
        /// <param name="dropHandle">Identifier of the structure that contains the file names of the dropped files (hDrop).</param>
        /// <param name="iFile">Index of the file to query.</param>
        /// <param name="file">The buffer that receives the file name of a dropped file when the function returns.</param>
        /// <param name="cch">The size, in characters, of the buffer.</param>
        /// <returns>When the function copies a file name to the buffer, the return value is a count of the characters copied, not including the terminating null character.</returns>
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        internal static extern int DragQueryFile(IntPtr dropHandle, int iFile, char[] file, int cch);

        /// <summary>
        /// Frees the specified storage medium.
        /// </summary>
        /// <param name="medium">Reference to the storage medium that is to be freed.</param>
        [DllImport("ole32.dll")]
        internal static extern void ReleaseStgMedium([In] ref STGMEDIUM medium);

        [DllImport("user32.dll")]
        internal static extern bool InsertMenuItem(IntPtr menu, uint uItem, bool byPosition, [In] ref MenuItemInfo menuItemInfo);

        [DllImport("user32.dll")]
        internal static extern IntPtr CreatePopupMenu();

        [DllImport("Comctl32.dll")]
        internal static extern IntPtr CreatePropertySheetPage(ref PropSheetPage propertySheetPage);
    }
}
