// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices.ComTypes;
using MiniShellFramework.ComTypes;

namespace MiniShellFramework
{
    /// <summary>
    /// Provide a base class for propery sheet shell extensions.
    /// </summary>
    public abstract class ShellExtensionInit : IShellExtInit
    {
        private readonly List<string> fileNames = new List<string>();

        void IShellExtInit.Initialize(IntPtr pidlFolder, IDataObject dataObject, uint hkeyProgId)
        {
            Debug.WriteLine("{0}.IShellExtInit.Initialize (ContextMenuBase)", this);
            CacheFiles(dataObject);
        }

        /// <summary>
        /// Gets the files names.
        /// </summary>
        /// <value>The files names.</value>
        protected IList<string> FilesNames
        {
            get { return fileNames; }
        }

        private void CacheFiles(IDataObject dataObject)
        {
            Contract.Requires(dataObject != null);

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
