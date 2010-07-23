// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Text;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace MiniShellFramework
{
    public enum ClipFormat
    {
        CF_HDROP = 15
    }

    public static class FormatEtcExtensions
    {
        public static FORMATETC CreateFORMATETC(ClipFormat clipFormat)
        {
            FORMATETC format = new FORMATETC();
            format.cfFormat = (short) clipFormat;
            format.tymed = TYMED.TYMED_HGLOBAL;
            format.dwAspect = DVASPECT.DVASPECT_CONTENT;
            format.ptd = IntPtr.Zero;
            format.lindex = -1;

            return format;
        }

        public static STGMEDIUM CreateSTGMEDIUM()
        {
            STGMEDIUM medium = new STGMEDIUM();

            return medium;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern int DragQueryFile(IntPtr hDrop, int iFile, char[] file, int cch);

        [DllImport("ole32.dll")]
        public static extern void ReleaseStgMedium([In] ref STGMEDIUM medium);
    }

    public class ClipboardFormatDrop : IDisposable
    {
        STGMEDIUM medium;

        public ClipboardFormatDrop(IDataObject dataObject)
        {
            FORMATETC format = FormatEtcExtensions.CreateFORMATETC(ClipFormat.CF_HDROP);

            dataObject.GetData(ref format, out medium);
        }

        int GetFileCount()
        {
            return FormatEtcExtensions.DragQueryFile(medium.unionmember, -1, null, 0);
        }

        public void Dispose()
        {
            FormatEtcExtensions.ReleaseStgMedium(ref medium);
        }

        public string GetFile(int index)
        {
            var builder = new StringBuilder();

            var buffer = new char[255 + 1]; // MAX_PATH
            int queryFileLength = FormatEtcExtensions.DragQueryFile(medium.unionmember, index, buffer, 255);
            if (queryFileLength <= 0)
                throw new Win32Exception();

            string filename = new String(buffer, 0, queryFileLength);
            return filename;
        }
    }
}
