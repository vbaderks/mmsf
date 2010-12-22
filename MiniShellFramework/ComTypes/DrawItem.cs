// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// TODO
    /// </summary>
    public enum OwnerDrawControlType
    {
        /// <summary>
        /// TODO
        /// </summary>
        Menu = 1,

        /// <summary>
        /// TODO
        /// </summary>
        ListBox = 2,

        /// <summary>
        /// TODO
        /// </summary>
        ComboBox = 3,

        /// <summary>
        /// TODO
        /// </summary>
        Button = 4,

        /// <summary>
        /// TODO
        /// </summary>
        Static = 5
    }

    /// <summary>
    /// TODO
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        /// <summary>
        /// 
        /// </summary>
        public int Left;

        /// <summary>
        /// 
        /// </summary>
        public int Top;

        /// <summary>
        /// 
        /// </summary>
        public int Right;

        /// <summary>
        /// 
        /// </summary>
        public int Bottom;
    }

    /// <summary>
    /// TODO
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DrawItem
    {
        /// <summary>
        /// 
        /// </summary>
        public OwnerDrawControlType CtlType;

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
        public uint itemAction;

        /// <summary>
        /// 
        /// </summary>
        public uint itemState;

        /// <summary>
        /// 
        /// </summary>
        public IntPtr hwndItem;

        /// <summary>
        /// 
        /// </summary>
        public IntPtr hDC;

        /// <summary>
        /// 
        /// </summary>
        public RECT rcItem;

        /// <summary>
        /// 
        /// </summary>
        public IntPtr itemData;
    }
}
