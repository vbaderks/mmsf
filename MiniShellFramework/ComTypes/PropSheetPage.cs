// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace MiniShellFramework.ComTypes
{
    public delegate bool DialogProc(IntPtr hwndDlg, uint uMsg, IntPtr wParam, IntPtr lParam);

    public delegate uint PropSheetCallback(IntPtr hwnd, uint uMsg, IntPtr ppsp);

    public enum PSP : uint
    {
        DEFAULT = 0x00000000,
        DLGINDIRECT = 0x00000001,
        USEHICON = 0x00000002,
        USEICONID = 0x00000004,
        USETITLE = 0x00000008,
        RTLREADING = 0x00000010,

        HASHELP = 0x00000020,
        USEREFPARENT = 0x00000040,
        USECALLBACK = 0x00000080,
        PREMATURE = 0x00000400,

        HIDEHEADER = 0x00000800,
        USEHEADERTITLE = 0x00001000,
        USEHEADERSUBTITLE = 0x00002000
    }

    /// <summary>
    /// TODO
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PropSheetPage
    {
        /// <summary>
        /// Size, in bytes, of this structure. The property sheet manager uses this member to determine which version of the structure you are using. (dwSize).
        /// </summary>
        public int Size;

        public PSP dwFlags;
        public IntPtr hInstance;
        public IntPtr pResource;
        public IntPtr hIcon;
        private string title;
        public DialogProc pfnDlgProc;
        public IntPtr lParam;
        public PropSheetCallback pfnCallback;
        public int pcRefParent;

        /// <summary>
        /// Title of the header area. (pszHeaderTitle).
        /// </summary>
        public string HeaderTitle;

        /// <summary>
        /// TSubtitle of the header area. (pszHeaderSubTitle).
        /// </summary>
        public string HeaderSubTitle;

        /// <summary>
        /// Initializes the specified info.
        /// </summary>
        public void InitializeSize()
        {
            Size = Marshal.SizeOf(typeof(PropSheetPage));
        }

        /// <summary>
        /// Sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            set
            {
                dwFlags |= PSP.USETITLE;
                title = value;
            }
        }
    }
}
