// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using MiniShellFramework.ComTypes;

namespace MiniShellFramework
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertySheetPage
    {
        private string title;
        protected IntPtr dlgTemplate;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySheetPage"/> class.
        /// </summary>
        public PropertySheetPage(string title)
        {
            this.title = title;
        }

        internal IntPtr Create()
        {
            var propSheetPage = new PropSheetPage();
            propSheetPage.InitializeSize();

            propSheetPage.dwFlags |= PropSheetPageOptions.DLGINDIRECT;

            return IntPtr.Zero;
        }

        private IntPtr GetDialogTemplate()
        {
            // If we're already created a template, don't bother doing it again
            if (dlgTemplate != IntPtr.Zero)
                return dlgTemplate;

            var dlg = new DLGTEMPLATE();

            // try to synth the template from the user control properties 
            // so the propertysheet owned by MMC can size itself properly

            ////Font fnt = MainControl.Font;
            return IntPtr.Zero;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Auto)]
    internal struct DLGTEMPLATE
    {
        internal uint style;
        internal uint dwExtendedStyle;
        internal ushort cdit;
        internal short x;
        internal short y;
        internal short cx;
        internal short cy;
        internal short wMenuResource;
        internal short wWindowClass;
        internal short wTitleArray;
        internal short wFontPointSize;
        [MarshalAs(UnmanagedType.LPWStr)]
        internal string szFontTypeface;
    }
}
