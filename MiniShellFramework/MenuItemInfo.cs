// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace MiniShellFramework
{
    /// <summary>
    /// (MENUITEMINFO)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MenuItemInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public uint cbSize;

        /// <summary>
        /// 
        /// </summary>
        public uint fMask;

        /// <summary>
        /// 
        /// </summary>
        public uint fType;           // used if MIIM_TYPE (4.0) or MIIM_FTYPE (>4.0)

        /// <summary>
        /// 
        /// </summary>
        public uint fState;          // used if MIIM_STATE

        /// <summary>
        /// 
        /// </summary>
        public uint wID;             // used if MIIM_ID

        /// <summary>
        /// 
        /// </summary>
        public IntPtr hSubMenu;      // used if MIIM_SUBMENU

        /// <summary>
        /// 
        /// </summary>
        public IntPtr hbmpChecked;   // used if MIIM_CHECKMARKS

        /// <summary>
        /// 
        /// </summary>
        public IntPtr hbmpUnchecked; // used if MIIM_CHECKMARKS

        /// <summary>
        /// 
        /// </summary>
        public IntPtr dwItemData;    // used if MIIM_DATA

        /// <summary>
        /// 
        /// </summary>
        public string dwTypeData;    // used if MIIM_TYPE (4.0) or MIIM_STRING (>4.0)

        /// <summary>
        /// 
        /// </summary>
        public uint cch;             // used if MIIM_TYPE (4.0) or MIIM_STRING (>4.0)

        /// <summary>
        /// 
        /// </summary>
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
        public static void Initialize(ref MenuItemInfo info)
        {
            info.cbSize = (uint)Marshal.SizeOf(typeof(MenuItemInfo));
            info.fMask = 0;
            info.fType = 0;
        }
    }
}
