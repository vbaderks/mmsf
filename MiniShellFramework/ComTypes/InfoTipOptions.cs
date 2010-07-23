// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Flags that define the handling of the item when retrieving the info tip text.
    /// </summary>
    [Flags]
    public enum InfoTipOptions
    {
        /// <summary>
        /// No special handling. QITIPF_DEFAULT (0).
        /// </summary>
        Default = 0,

        /// <summary>
        /// Provide the name of the item in ppwszTip rather than the info tip text. QITIPF_USENAME (1).
        /// </summary>
        UseName = 1,

        /// <summary>
        /// If the item is a shortcut, retrieve the info tip text of the shortcut rather than its target. QITIPF_LINKNOTARGET (2).
        /// </summary>
        LinkNoTarget = 2,

        /// <summary>
        /// If the item is a shortcut, retrieve the info tip text of the shortcut's target. QITIPF_LINKUSETARGET (4).
        /// </summary>
        LinkUseTarget = 4,

        /// <summary>
        /// Search the entire namespace for the information. This may result in a delayed response time. QITIPF_USESLOWTIP (8).
        /// </summary>
        UseSlowTip = 8,

        /// <summary>
        /// Put the info tip on a single line (only Windows Vista and later). QITIPF_SINGLELINE (10).
        /// </summary>
        SingleLine = 0x10
    }
}