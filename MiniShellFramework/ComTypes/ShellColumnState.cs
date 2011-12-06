// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Flags indicating the default column state (SHCOLSTATE).
    /// </summary>
    [Flags]
    public enum ShellColumnState
    {
        /// <summary>
        /// Default (SHCOLSTATE_DEFAULT).
        /// </summary>
        Default = 0,

        /// <summary>
        /// Data type is a string (SHCOLSTATE_TYPE_STR).
        /// </summary>
        TypeString = 0x1,

        /// <summary>
        /// Data type is a integer (SHCOLSTATE_TYPE_INT).
        /// </summary>
        TypeInteger = 0x2,

        /// <summary>
        /// Data type is a integer (SHCOLSTATE_TYPE_DATE).
        /// </summary>
        TypeDate = 0x3,

        /// <summary>
        /// TODO (SHCOLSTATE_SLOW).
        /// </summary>
        Slow = 0x20,

        /// <summary>
        /// Provided by a handler, not the folder (SHCOLSTATE_EXTENDED)
        /// </summary>
        Extended = 0x40,

        /// <summary>
        /// Not displayed in context menu, but listed in the "More..." dialog (SHCOLSTATE_SECONDARYUI).
        /// </summary>
        SecondaryUI = 0x80
    }

    ////SHCOLSTATE_TYPEMASK = 0xf,
    ////SHCOLSTATE_ONBYDEFAULT = 0x10,
    ////SHCOLSTATE_HIDDEN = 0x100,
    ////SHCOLSTATE_PREFER_VARCMP = 0x200,
    ////SHCOLSTATE_PREFER_FMTCMP = 0x400,
    ////SHCOLSTATE_NOSORTBYFOLDERNESS = 0x800,
    ////SHCOLSTATE_VIEWONLY = 0x10000,
    ////SHCOLSTATE_BATCHREAD = 0x20000,
    ////SHCOLSTATE_NO_GROUPBY = 0x40000,
    ////SHCOLSTATE_FIXED_WIDTH = 0x1000,
    ////SHCOLSTATE_NODPISCALE = 0x2000,
    ////SHCOLSTATE_FIXED_RATIO = 0x4000,
    ////SHCOLSTATE_DISPLAYMASK = 0xf000
}
