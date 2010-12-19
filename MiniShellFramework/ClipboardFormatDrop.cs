// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace MiniShellFramework
{
    /// <summary>Support class to handle the CF_HDROP format.</summary>
    /// <remarks>
    /// The CF_HDROP format is used by the shell to transfer a group of existing files.
    /// The handle refers to a set of DROPFILES structures.
    /// </remarks>
    public sealed class ClipboardFormatDrop : IDisposable
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
            SafeNativeMethods.ReleaseStgMedium(ref medium);

            // TODO: add finalizer to ensure unmanaged memory is always released.
        }

        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>A string with the file name.</returns>
        public string GetFile(int index)
        {
            var builder = new StringBuilder();

            var buffer = new char[255 + 1]; // MAX_PATH
            int queryFileLength = SafeNativeMethods.DragQueryFile(medium.unionmember, index, buffer, 255);
            Contract.Assume(queryFileLength <= 255);
            if (queryFileLength <= 0)
                throw new Win32Exception();

            return new string(buffer, 0, queryFileLength);
        }

        /// <summary>
        /// Gets the file count.
        /// </summary>
        /// <returns>Count of files.</returns>
        public int GetFileCount()
        {
            return SafeNativeMethods.DragQueryFile(medium.unionmember, -1, null, 0);
        }
    }
}
