// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;

namespace MiniShellFramework.Interfaces
{
    [Flags]
    public enum InfoTipOptions
    {
        Default = 0,
        UseName = 1,
        LinkNoTarget = 2,
        LinkUseTarget = 4,
        UseSlowTip = 8,
        SingleLine = 0x10 // only on Vista or higer
    }
}