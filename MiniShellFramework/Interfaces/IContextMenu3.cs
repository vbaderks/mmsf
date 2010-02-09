// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MiniShellFramework.Interfaces
{
    [Flags]
    public enum QueryContextMenuOptions
    {
        // CMF_NORMAL
        Normal = 0x00000000,

        // CMF_DEFAULTONLY
        DefaultOnly = 0x00000001,

        // CMF_VERBSONLY
        VerbsOnly = 0x00000002,

        // CMF_EXPLORE
        Explore = 0x00000004,

        // CMF_NOVERBS
        NoVerbs = 0x00000008,

        // CMF_CANRENAME
        CanRename = 0x00000010,

        // CMF_NODEFAULT
        NoDefault = 0x00000020
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct InvokeCommandInfo  // CMINVOKECOMMANDINFO
    {
        public int cbSize;           // sizeof(CMINVOKECOMMANDINFO)
        public int fMask;            // any combination of CMIC_MASK_*
        public IntPtr hwnd;          // might be NULL (indicating no owner window)
        public IntPtr lpVerb;        // either a string or MAKEINTRESOURCE(idOffset)
        public string lpParameters;  // might be NULL (indicating no parameter)
        public string lpDirectory;   // might be NULL (indicating no specific directory)
        public int nShow;            // one of SW_ values for ShowWindow() API
        public int dwHotKey;
        public IntPtr hIcon;
    }

    [ComImport()]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [GuidAttribute("bcfce0a0-ec17-11d0-8d10-00a0c90f2719")]
    public interface IContextMenu3
    {
        // IContextMenu
        [PreserveSig()]
        int QueryContextMenu(IntPtr hMenu, uint iMenu, uint idCmdFirst, uint idCmdLast, QueryContextMenuOptions flags);

        void InvokeCommand([In] ref InvokeCommandInfo invokeCommandInfo);

        [PreserveSig()]
        void GetCommandString(int idCmd, uint uflags, int reserved, StringBuilder commandString, int cch);

        // IContextMenu2
        [PreserveSig()]
        int HandleMenuMsg(uint uMsg, uint wParam, uint lParam);

        // IContextMenu3
        [PreserveSig()]
        int HandleMenuMsg2(uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr plResult);
    }
}
