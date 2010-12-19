// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Indicates the members to be retrieved or set from a MenuItemInfo struct.
    /// </summary>
    [Flags]
    public enum MenuItemInfoMask
    {
        /// <summary>
        /// Retrieves or sets the fState member (MIIM_STATE).
        /// </summary>
        State = 0x1,

        /// <summary>
        /// Retrieves or sets the wID member (MIIM_ID).
        /// </summary>
        Id = 0x2,

        /// <summary>
        /// Retrieves or sets the hSubMenu member (MIIM_SUBMENU).
        /// </summary>
        SubMenu = 0x4,

        /// <summary>
        /// Retrieves or sets the hbmpChecked and hbmpUnchecked members (MIIM_CHECKMARKS).
        /// </summary>
        CheckMarks = 0x8,

        /// <summary>
        /// Retrieves or sets the dwItemData member (MIIM_DATA).
        /// </summary>
        Data = 0x20,

        /// <summary>
        /// Retrieves or sets the dwTypeData member (MIIM_STRING).
        /// </summary>
        String = 0x40,

        /// <summary>
        /// Retrieves or sets the hbmpItem member (MIIM_BITMAP).
        /// </summary>
        Bitmap = 0x80,

        /// <summary>
        /// Retrieves or sets the fType member (MIIM_FTYPE).
        /// </summary>
        FType = 0x100
    }
}
