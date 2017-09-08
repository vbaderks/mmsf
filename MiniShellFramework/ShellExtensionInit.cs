// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using MiniShellFramework.ComTypes;

namespace MiniShellFramework
{
    /// <inheritdoc cref="ShellExtension" />
    /// <summary>
    /// Provide a base class for property sheet shell extensions.
    /// </summary>
    [ComVisible(true)] // Make this .NET class COM visible to ensure derived class can be COM visible.
    public abstract class ShellExtensionInit : ShellExtension, IShellExtInit
    {
        private readonly List<string> fileNames = new List<string>();
        private readonly List<string> extensions = new List<string>();

        /// <summary>
        /// Gets the files names.
        /// </summary>
        /// <value>The files names.</value>
        protected IList<string> FilesNames => fileNames;

        void IShellExtInit.Initialize(IntPtr pidlFolder, IDataObject dataObject, uint hkeyProgId)
        {
            Debug.WriteLine("[{0}] ShellExtensionInit.IShellExtInit.Initialize (pidlFolder={1}, dataObject={2}, hkeyProgId={3})",
                Id, pidlFolder, dataObject, hkeyProgId);
            CacheFiles(dataObject);
        }

        /// <summary>
        /// Registers a file extension.
        /// </summary>
        /// <param name="extension">The file extension.</param>
        protected void RegisterExtension(string extension)
        {
            extensions.Add(extension.ToUpperInvariant());
        }

        /// <summary>
        /// Determines whether [contains unknown extension] [the specified file names].
        /// </summary>
        /// <param name="names">The file names.</param>
        /// <returns>
        /// <c>true</c> if [contains unknown extension] [the specified file names]; otherwise, <c>false</c>.
        /// </returns>
        protected bool ContainsUnknownExtension(IEnumerable<string> names)
        {
            return fileNames.Any(IsUnknownExtension);
        }

        /// <summary>
        /// Determines whether [is unknown extension] [the specified file name].
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>
        /// <c>true</c> if [is unknown extension] [the specified file name]; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsUnknownExtension(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            var extension = Path.GetExtension(fileName).ToUpperInvariant();
            return extensions.FindIndex(x => x == extension) == -1;
        }

        private void CacheFiles(IDataObject dataObject)
        {
            fileNames.Clear();

            using (var clipboardFormatDrop = new ClipboardFormatDrop(dataObject))
            {
                var count = clipboardFormatDrop.GetFileCount();
                for (int i = 0; i < count; i++)
                {
                    fileNames.Add(clipboardFormatDrop.GetFile(i));
                }
            }
        }
    }
}
