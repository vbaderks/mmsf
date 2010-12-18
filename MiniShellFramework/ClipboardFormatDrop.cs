// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace MiniShellFramework
{
    /// <summary>
    /// Definitions of Shell Clipboard Formats.
    /// </summary>
    public enum ClipFormat
    {
        /// <summary>
        /// This clipboard format is used when transferring the locations of a group of existing files.
        /// </summary>
        CF_HDROP = 15
    }

    /// <summary>
    /// TODO
    /// </summary>
    public static class FormatEtcExtensions
    {
        /// <summary>
        /// Creates the FORMATETC.
        /// </summary>
        /// <param name="clipFormat">The clip format.</param>
        /// <returns></returns>
        public static FORMATETC CreateFORMATETC(ClipFormat clipFormat)
        {
            FORMATETC format = new FORMATETC();
            format.cfFormat = (short)clipFormat;
            format.tymed = TYMED.TYMED_HGLOBAL;
            format.dwAspect = DVASPECT.DVASPECT_CONTENT;
            format.ptd = IntPtr.Zero;
            format.lindex = -1;

            return format;
        }

        /// <summary>
        /// Creates the STGMEDIUM.
        /// </summary>
        /// <returns></returns>
        public static STGMEDIUM CreateSTGMEDIUM()
        {
            STGMEDIUM medium = new STGMEDIUM();

            return medium;
        }

        /// <summary>
        /// Drags the query file.
        /// </summary>
        /// <param name="hDrop">The h drop.</param>
        /// <param name="iFile">The i file.</param>
        /// <param name="file">The file.</param>
        /// <param name="cch">The CCH.</param>
        /// <returns></returns>
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern int DragQueryFile(IntPtr hDrop, int iFile, char[] file, int cch);

        /// <summary>
        /// Releases the STG medium.
        /// </summary>
        /// <param name="medium">The medium.</param>
        [DllImport("ole32.dll")]
        public static extern void ReleaseStgMedium([In] ref STGMEDIUM medium);
    }

    /// <summary>Support class to handle the CF_HDROP format.</summary>
    /// <remarks>
    /// The CF_HDROP format is used by the shell to transfer a group of existing files.
    /// The handle refers to a set of DROPFILES structures.
    /// </remarks>
    public class ClipboardFormatDrop : IDisposable
    {
        private STGMEDIUM medium;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipboardFormatDrop"/> class.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        public ClipboardFormatDrop(IDataObject dataObject)
        {
            Contract.Requires(dataObject != null);

            FORMATETC format = FormatEtcExtensions.CreateFORMATETC(ClipFormat.CF_HDROP);
            dataObject.GetData(ref format, out medium);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            FormatEtcExtensions.ReleaseStgMedium(ref medium);

            // TODO: add finalizer to ensure unmanaged memory is always released.
        }

        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public string GetFile(int index)
        {
            var builder = new StringBuilder();

            var buffer = new char[255 + 1]; // MAX_PATH
            int queryFileLength = FormatEtcExtensions.DragQueryFile(medium.unionmember, index, buffer, 255);
            Contract.Assume(queryFileLength <= 255);
            if (queryFileLength <= 0)
                throw new Win32Exception();

            return new string(buffer, 0, queryFileLength);
        }

        /// <summary>
        /// Gets the file count.
        /// </summary>
        /// <returns></returns>
        public int GetFileCount()
        {
            return FormatEtcExtensions.DragQueryFile(medium.unionmember, -1, null, 0);
        }
    }
}
