// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Flag options for the IContextMenu interface.
    /// </summary>
    [Flags]
    public enum QueryContextMenuOptions
    {
        /// <summary>
        /// Indicates normal operation. A shortcut menu extension, namespace extension,
        /// or drag-and-drop handler can add all menu items. (CMF_NORMAL).
        /// </summary>
        Normal = 0x00000000,

        /// <summary>
        /// The user is activating the default action, typically by double-clicking.
        /// This flag provides a hint for the shortcut menu extension to add nothing if it 
        /// does not modify the default item in the menu. A shortcut menu extension or
        /// drag-and-drop handler should not add any menu items if this value is specified.
        /// A namespace extension should at most add only the default item. (CMF_DEFAULTONLY).
        /// </summary>
        DefaultOnly = 0x00000001,

        /// <summary>
        /// The shortcut menu is that of a shortcut file (normally, a .lnk file).
        /// Shortcut menu handlers should ignore this value. (CMF_VERBSONLY).
        /// </summary>
        VerbsOnly = 0x00000002,

        /// <summary>
        /// The Windows Explorer tree window is present. (CMF_EXPLORE).
        /// </summary>
        Explore = 0x00000004,

        /// <summary>
        /// This flag is set for items displayed in the Send To menu.
        /// Shortcut menu handlers should ignore this value. (CMF_NOVERBS).
        /// </summary>
        NoVerbs = 0x00000008,

        /// <summary>
        /// The calling application supports renaming of items.
        /// A shortcut menu or drag-and-drop handler should ignore this flag.
        /// A namespace extension should add a Rename item to the menu if applicable. (CMF_CANRENAME).
        /// </summary>
        CanRename = 0x00000010,

        /// <summary>
        /// No item in the menu has been set as the default.
        /// A drag-and-drop handler should ignore this flag.
        /// A namespace extension should not set any of the menu items as the default. (CMF_NODEFAULT)
        /// </summary>
        NoDefault = 0x00000020
    }
}
