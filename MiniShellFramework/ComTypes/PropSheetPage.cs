// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace MiniShellFramework.ComTypes
{
    public delegate bool DialogProc(IntPtr hwndDlg, uint uMsg, IntPtr wParam, IntPtr lParam);

    public delegate uint PropSheetCallback(IntPtr hwnd, uint uMsg, IntPtr ppsp);

    /// <summary>
    /// Flags that indicate which options to use when creating the property sheet page (PSP).
    /// </summary>
    public enum PropSheetPageOptions : uint
    {
        /// <summary>
        /// Uses the default meaning for all structure members. This flag is not supported when using the Aero-style wizard 
        /// </summary>
        DEFAULT = 0x00000000,

        /// <summary>
        /// Creates the page from the dialog box template in memory pointed to by the pResource member.
        /// The PropertySheet function assumes that the template that is in memory is not write-protected.
        /// A read-only template will cause an exception in some versions of Windows. 
        /// </summary>
        DLGINDIRECT = 0x00000001,

        /// <summary>
        /// Uses hIcon as the small icon on the tab for the page. This flag is not supported when using the Aero-style wizard
        /// </summary>
        USEHICON = 0x00000002,

        /// <summary>
        /// Uses pszIcon as the name of the icon resource to load and use as the small icon on the tab for the page.
        /// This flag is not supported when using the Aero-style wizard
        /// </summary>
        USEICONID = 0x00000004,

        /// <summary>
        /// Uses the pszTitle member as the title of the property sheet dialog box instead of the title stored 
        /// in the dialog box template. This flag is not supported when using the Aero-style wizard 
        /// </summary>
        USETITLE = 0x00000008,

        /// <summary>
        /// Reverses the direction in which pszTitle is displayed. Normal windows display all text, including pszTitle,
        /// left-to-right (LTR). For languages such as Hebrew or Arabic that read right-to-left (RTL),
        /// a window can be mirrored and all text will be displayed RTL. If PSP_RTLREADING is set, pszTitle will 
        /// instead read RTL in a normal parent window, and LTR in a mirrored parent window. 
        /// </summary>
        RTLREADING = 0x00000010,

        /// <summary>
        /// Enables the property sheet Help button when the page is active. This flag is not supported when using the Aero-style wizard.
        /// </summary>
        HASHELP = 0x00000020,

        /// <summary>
        /// 
        /// </summary>
        USEREFPARENT = 0x00000040,

        /// <summary>
        /// 
        /// </summary>
        USECALLBACK = 0x00000080,

        /// <summary>
        /// 
        /// </summary>
        PREMATURE = 0x00000400,

        /// <summary>
        /// 
        /// </summary>
        HIDEHEADER = 0x00000800,
        
        /// <summary>
        /// 
        /// </summary>
        USEHEADERTITLE = 0x00001000,
        
        /// <summary>
        /// 
        /// </summary>
        USEHEADERSUBTITLE = 0x00002000
    }

    /// <summary>
    /// Defines a page in a property sheet (PROPSHEETPAGE).
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PropSheetPage
    {
        /// <summary>
        /// Size, in bytes, of this structure. The property sheet manager uses this member to determine which version of the structure you are using (dwSize).
        /// </summary>
        public int Size;

        /// <summary>
        /// Flags that indicate which options to use when creating the property sheet page (dwFlags).
        /// </summary>
        public PropSheetPageOptions dwFlags;

        /// <summary>
        /// Handle to the instance from which to load an icon or string resource (hInstance).
        /// </summary>
        public IntPtr InstanceHandle;

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
                dwFlags |= PropSheetPageOptions.USETITLE;
                title = value;
            }
        }
    }
}
