﻿// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Microsoft.Win32;
using MiniShellFramework.ComTypes;

namespace MiniShellFramework
{
    /// <summary>
    /// Base class for Context Menu shell extension handlers.
    /// </summary>
    [ComVisible(true)]                        // Make this .NET class COM visible to ensure derived class can be COM visible.
    [ClassInterface(ClassInterfaceType.None)] // Only the functions from the COM interfaces should be accessible.
    public abstract class ContextMenuBase : IShellExtInit, IContextMenu3
    {
        private uint idCmdFirst;
        private readonly List<string> extensions = new List<string>();
        private readonly List<string> fileNames = new List<string>();
        private readonly List<MenuItem> menuItems = new List<MenuItem>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextMenuBase"/> class.
        /// </summary>
        protected ContextMenuBase()
        {
            Debug.WriteLine("ContextMenuBase::Constructor (instance={0})", this);
        }

        void IShellExtInit.Initialize(IntPtr pidlFolder, IDataObject dataObject, uint hkeyProgId)
        {
            Debug.WriteLine("{0}.IShellExtInit.Initialize (ContextMenuBase)", this);
            CacheFiles(dataObject);
        }

        int IContextMenu3.QueryContextMenu(IntPtr hmenu, uint indexMenu, uint idCommandFirst, uint idCmdLast, QueryContextMenuOptions flags)
        {
            Debug.WriteLine("ContextMenuBase::QueryContextMenu (instance={0}, indexMenu={1}, idCmdFirst={2}, idLast={3}, flag={4})",
                this, indexMenu, idCmdFirst, idCmdLast, flags);

            // If the flags include CMF_DEFAULTONLY then we shouldn't do anything.
            if (flags.HasFlag(QueryContextMenuOptions.DefaultOnly))
                return HResults.Create(Severity.Success, 0);

            ////ClearMenuItems();

            this.idCmdFirst = idCommandFirst;
            uint id = idCmdFirst;
            ////CMenu menu(hmenu, indexMenu, nID, idCmdLast, this);

            ////static_cast<T*>(this)->OnQueryContextMenu(menu, GetFilenames());
            var menu = new Menu(hmenu, indexMenu, idCmdFirst, idCmdLast);
            QueryContextMenuCore(menu, null);

            return HResults.Create(Severity.Success, (ushort)(id - idCmdFirst));
        }

        void IContextMenu3.InvokeCommand(ref InvokeCommandInfo invokeCommandInfo)
        {
            Debug.WriteLine("ContextMenuBase::InvokeCommand (instance={0})", this);

            ////if (HIWORD(pici->lpVerb) != 0)
            if (invokeCommandInfo.lpVerb.ToInt32() != 0)
                throw new ArgumentException("Verbs not supported");

            ////GetMenuItem(LOWORD(pici->lpVerb)).GetContextCommand()(pici, GetFilenames());
        }

        ////public void InvokeCommand(IntPtr pici)
        ////{
        ////    throw new NotImplementedException();
        ////}

        int IContextMenu3.GetCommandString(IntPtr idCommand, uint uflags, int reserved, StringBuilder commandString, int cch)
        {
            throw new NotImplementedException();
        }

        void IContextMenu3.HandleMenuMsg(uint uMsg, uint wParam, uint lParam)
        {
            throw new NotImplementedException();
        }

        void IContextMenu3.HandleMenuMsg2(uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr plResult)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds additional info to the registry to allow the shell to discover the oject as shell extension.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="description">The description.</param>
        /// <param name="progId">The prog id.</param>
        protected static void ComRegister(Type type, string description, string progId)
        {
            // Register the ContextMenu COM object as an approved shell extension. Explorer will only execute approved extensions.
            using (var key =
                Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved", true))
            {
                key.SetValue(type.GUID.ToString("B"), description);
            }

            // Register the ContextMenu COM object as the ContextMenu handler.
            using (var key = Registry.ClassesRoot.CreateSubKey(progId + @"\ShellEx\ContextMenuHandlers\" + description))
            {
                key.SetValue(string.Empty, type.GUID.ToString("B"));
            }
        }

        /// <summary>
        /// Removed the additional info from the registry that allowed the shell to discover the shell extension.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="description">The description.</param>
        /// <param name="progId">The prog id.</param>
        protected static void ComUnregister(Type type, string description, string progId)
        {
            // Unregister the ContextMenu COM object as an approved shell extension.
            using (var key =
                Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved", true))
            {
                key.DeleteValue(type.GUID.ToString("B"));
            }

            // Note: prog id should be removed by 1 global unregister function.
        }

        /// <summary>
        /// Queries the context menu core.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="filenames">The filenames.</param>
        protected abstract void QueryContextMenuCore(Menu menu, IList<string> filenames);

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
        /// <param name="fileNames">The file names.</param>
        /// <returns>
        /// <c>true</c> if [contains unknown extension] [the specified file names]; otherwise, <c>false</c>.
        /// </returns>
        protected bool ContainsUnknownExtension(IEnumerable<string> fileNames)
        {
            Contract.Requires(fileNames != null);
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
            Contract.Requires(fileName != null);
            var extension = Path.GetExtension(fileName).ToUpperInvariant();
            return extensions.FindIndex(x => x == extension) == -1;
        }

        // 'IMenuHost'
        internal void OnAddMenuItem(string helpText, Menu.ContextCommand contextCommand, CustomMenuHandler customMenuHandler)
        {
            menuItems.Add(new MenuItem(helpText, contextCommand, customMenuHandler));
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

        private class MenuItem
        {
            private string helpText;
            private Menu.ContextCommand contextcommand;
            private CustomMenuHandler custommenuhandler;

            public MenuItem(string helpText, Menu.ContextCommand contextcommand, CustomMenuHandler custommenuhandler)
            {
                this.helpText = helpText;
                this.contextcommand = contextcommand;
                this.custommenuhandler = custommenuhandler;
            }
        }
    }
}
