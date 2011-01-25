// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Flags indicating the default column state (SHCOLSTATE).
    /// </summary>
    public enum ShellColumnState
    {
        /// <summary>
        /// Default (SHCOLSTATE_DEFAULT).
        /// </summary>
        Default = 0,

        /// <summary>
        /// Data type is a string (SHCOLSTATE_TYPE_STR).
        /// </summary>
        TypeString = 1,

        /// <summary>
        /// Data type is a integer (SHCOLSTATE_TYPE_INT).
        /// </summary>
        TypeInteger = 2,

        /// <summary>
        /// Data type is a integer (SHCOLSTATE_TYPE_DATE).
        /// </summary>
        TypeDate = 3
    }

    //SHCOLSTATE_TYPEMASK	= 0xf,
    //SHCOLSTATE_ONBYDEFAULT	= 0x10,
    //SHCOLSTATE_SLOW	= 0x20,
    //SHCOLSTATE_EXTENDED	= 0x40,
    //SHCOLSTATE_SECONDARYUI	= 0x80,
    //SHCOLSTATE_HIDDEN	= 0x100,
    //SHCOLSTATE_PREFER_VARCMP	= 0x200,
    //SHCOLSTATE_PREFER_FMTCMP	= 0x400,
    //SHCOLSTATE_NOSORTBYFOLDERNESS	= 0x800,
    //SHCOLSTATE_VIEWONLY	= 0x10000,
    //SHCOLSTATE_BATCHREAD	= 0x20000,
    //SHCOLSTATE_NO_GROUPBY	= 0x40000,
    //SHCOLSTATE_FIXED_WIDTH	= 0x1000,
    //SHCOLSTATE_NODPISCALE	= 0x2000,
    //SHCOLSTATE_FIXED_RATIO	= 0x4000,
    //SHCOLSTATE_DISPLAYMASK	= 0xf000
}
