// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Contains information about a menu item (MENUITEMINFO).
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MenuItemInfo
    {
        /// <summary>
        /// The size of the structure, in bytes. The caller must set this member to sizeof(MENUITEMINFO) (cbSize).
        /// </summary>
        public uint Size;

        /// <summary>
        /// Indicates the members to be retrieved or set (fMask).
        /// </summary>
        public MenuItemInfoMask Mask;

        /// <summary>
        /// 
        /// </summary>
        public MenuItemInfoType Type;           // used if MIIM_TYPE (4.0) or MIIM_FTYPE (>4.0)

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
        private IntPtr dwItemData;    // used if MIIM_DATA

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
                Mask |= MenuItemInfoMask.Id;
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
                Mask |= MenuItemInfoMask.String;
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
                    Type |= MenuItemInfoType.OwnerDraw;
                }
            }
        }

        /// <summary>
        /// Sets the sub menu.
        /// </summary>
        /// <value>The sub menu.</value>
        public IntPtr SubMenu
        {
            set
            {
                Mask |= MenuItemInfoMask.SubMenu;
                hSubMenu = value;
            }
        }

        /// <summary>
        /// Initializes the specified info.
        /// </summary>
        public void InitializeSize()
        {
            Size = (uint)Marshal.SizeOf(typeof(MenuItemInfo));
        }

        /// <summary>
        /// Sets the check marks.
        /// </summary>
        /// <param name="handleChecked">The handle checked.</param>
        /// <param name="handleUnchecked">The handle unchecked.</param>
        public void SetCheckMarks(IntPtr handleChecked, IntPtr handleUnchecked)
        {
            Mask |= MenuItemInfoMask.CheckMarks;
            hbmpChecked = handleChecked;
            hbmpUnchecked = handleUnchecked;
        }
    }
}
