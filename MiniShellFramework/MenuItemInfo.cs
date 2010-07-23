// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace MiniShellFramework
{
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MENUITEMINFO
    {
        public uint cbSize;
        public uint fMask;
        public uint fType;           // used if MIIM_TYPE (4.0) or MIIM_FTYPE (>4.0)
        public uint fState;          // used if MIIM_STATE
        public uint wID;             // used if MIIM_ID
        public IntPtr hSubMenu;      // used if MIIM_SUBMENU
        public IntPtr hbmpChecked;   // used if MIIM_CHECKMARKS
        public IntPtr hbmpUnchecked; // used if MIIM_CHECKMARKS
        public IntPtr dwItemData;    // used if MIIM_DATA
        public string dwTypeData;    // used if MIIM_TYPE (4.0) or MIIM_STRING (>4.0)
        public uint cch;             // used if MIIM_TYPE (4.0) or MIIM_STRING (>4.0)
        public IntPtr hbmpItem;      // used if MIIM_BITMAP

        /// <summary>
        /// Sets the id.
        /// </summary>
        /// <value>The id.</value>
        public uint Id
        {
            set
            {
                //// fMask |= MIIM_ID;
                wID = value;
            }
        }

        /// <summary>
        /// Sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            set
            {
                //// fMask |= MIIM_TYPE;
                //// fType |= MFT_STRING;
                dwTypeData = value;
            }
        }

        /// <summary>
        /// Sets a value indicating whether [owner draw].
        /// </summary>
        /// <value><c>true</c> if [owner draw]; otherwise, <c>false</c>.</value>
        public bool OwnerDraw
        {
            set
            {
                if (value)
                {
                    //// fType |= MFT_OWNERDRAW;
                }
            }
        }

        /// <summary>
        /// Initializes the specified info.
        /// </summary>
        /// <param name="info">The info.</param>
        public static void Initialize(ref MENUITEMINFO info)
        {
            info.cbSize = (uint)Marshal.SizeOf(typeof(MENUITEMINFO));
            info.fMask = 0;
            info.fType = 0;
        }
    }
}
