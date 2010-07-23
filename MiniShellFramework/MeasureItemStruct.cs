// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace MiniShellFramework
{
    /// <summary>
    /// (MEASUREITEMSTRUCT).
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MEASUREITEMSTRUCT
    {
        /// <summary>
        /// 
        /// </summary>
        public uint CtlType;

        /// <summary>
        /// 
        /// </summary>
        public uint CtlID;

        /// <summary>
        /// 
        /// </summary>
        public uint itemID;

        /// <summary>
        /// 
        /// </summary>
        public uint itemWidth;

        /// <summary>
        /// 
        /// </summary>
        public uint itemHeight;

        /// <summary>
        /// 
        /// </summary>
        public IntPtr itemData;
    }
}
