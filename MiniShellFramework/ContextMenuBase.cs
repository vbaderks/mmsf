// <copyright>
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
    [StructLayout(LayoutKind.Sequential)]
    struct MENUITEMINFO
    {
        public uint cbSize;
        public uint fMask;
        public uint fType;
        public uint fState;
        public uint wID;
        public IntPtr hSubMenu;
        public IntPtr hbmpChecked;
        public IntPtr hbmpUnchecked;
        public IntPtr dwItemData;
        public string dwTypeData;
        public uint cch;
        public IntPtr hbmpItem;

        // return the size of the structure
        public static uint sizeOf
        {
            get { return (uint)Marshal.SizeOf(typeof(MENUITEMINFO)); }
        }
    }

    /// <summary>
    /// Base class for Context Menu shell extension handlers.
    /// </summary>
    [ComVisible(true)]                        // Make this .NET class COM visible to ensure derived class can be COM visible.
    [ClassInterface(ClassInterfaceType.None)] // Only the functions from the COM interfaces should be accessible.
    public abstract class ContextMenuBase : IShellExtInit, IContextMenu3, IMenuHost
    {
        private uint currentCommandId;
        private readonly List<string> extensions = new List<string>();
        private readonly List<string> fileNames = new List<string>();
        private readonly List<MenuItem> menuItems = new List<MenuItem>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextMenuBase"/> class.
        /// </summary>
        protected ContextMenuBase()
        {
            Debug.WriteLine("{0}.Constructor (ContextMenuBase)", this);
        }

        void IShellExtInit.Initialize(IntPtr pidlFolder, IDataObject dataObject, uint hkeyProgId)
        {
            Debug.WriteLine("{0}.IShellExtInit.Initialize (ContextMenuBase)", this);
            CacheFiles(dataObject);
        }

        [DllImport("user32.dll")]
        static extern bool InsertMenuItem(IntPtr menu, uint uItem, bool byPosition, [In] ref MenuItemInfo menuItemInfo);

        int IContextMenu.QueryContextMenu(IntPtr menuHandle, uint position, uint firstCommandId, uint lastCommandId, QueryContextMenuOptions flags)
        {
            Debug.WriteLine("{0}.IContextMenu.QueryContextMenu (ContextMenuBase), position={1}, firstCommandId={2}, lastCommandId={3}, flag={4})",
                this, position, firstCommandId, lastCommandId, flags);

            if (flags.HasFlag(QueryContextMenuOptions.DefaultOnly))
                return HResults.Create(Severity.Success, 0); // don't add anything, only default menu items allowed.

            menuItems.Clear();

            currentCommandId = firstCommandId;

            MenuItemInfo mii = new MenuItemInfo();
            mii.InitializeSize();
            mii.Id = currentCommandId;
            mii.Text = "Send file(s) to CheapFTP member(s)";

            //MENUITEMINFO mii = new MENUITEMINFO();
            //mii.cbSize = 48;
            //mii.fMask = (uint)MenuItemInfoMask.Id | (uint)MenuItemInfoMask.String | (uint)MenuItemInfoMask.State;
            //mii.wID = commandId;
            ////mii.fType = (uint)MF.STRING;
            //mii.dwTypeData = "Send file(s) to CheapFTP member(s)";
            //mii.fState = (uint)MF.ENABLED;
            // Add it to the item
            var result = InsertMenuItem(menuHandle, position, true, ref mii);
            currentCommandId++;

            //QueryContextMenuCore(new Menu(hmenu, indexMenu, idCmdLast, this), this.fileNames);

            return HResults.Create(Severity.Success, (ushort)(currentCommandId - firstCommandId));
        }

        void IContextMenu.InvokeCommand(ref InvokeCommandInfo invokeCommandInfo)
        {
            Debug.WriteLine("ContextMenuBase::InvokeCommand (instance={0})", this);

            ////if (HIWORD(pici->lpVerb) != 0)
            if (invokeCommandInfo.lpVerb.ToInt32() != 0)
                throw new ArgumentException("Verbs not supported");

            ////GetMenuItem(LOWORD(pici->lpVerb)).GetContextCommand()(pici, GetFilenames());
        }

        int IContextMenu.GetCommandString(IntPtr commandId, GetCommandStringOptions flags, int reserved, IntPtr name, int cch)
        {
            Debug.WriteLine("{0}.IContextMenu.GetCommandString (ContextMenuBase), commandId={1}, flags={2}, name={3}, cch={4}", this, commandId, flags, name, cch);

            if (flags == GetCommandStringOptions.HelpText)
            {
                //commandString.Append("help text", 0, cch);
                return HResults.OK;
            }

            if (flags == GetCommandStringOptions.Verb)
            {
                return HResults.OK;
            }

            throw new NotSupportedException();
        }

        void IContextMenu3.HandleMenuMsg(uint uMsg, uint wParam, uint lParam)
        {
            throw new NotImplementedException();
        }

        int IContextMenu3.QueryContextMenu(IntPtr hmenu, uint indexMenu, uint idCommandFirst, uint idCmdLast, QueryContextMenuOptions flags)
        {
            return ((IContextMenu)this).QueryContextMenu(hmenu, indexMenu, idCommandFirst, idCmdLast, flags);
        }

        void IContextMenu3.InvokeCommand(ref InvokeCommandInfo invokeCommandInfo)
        {
            ((IContextMenu)this).InvokeCommand(ref invokeCommandInfo);
        }

        int IContextMenu3.GetCommandString(IntPtr idCommand, GetCommandStringOptions uflags, int reserved, IntPtr name, int cch)
        {
            return ((IContextMenu)this).GetCommandString(idCommand, uflags, reserved, name, cch);
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

        uint IMenuHost.GetCommandId()
        {
            return currentCommandId;
        }

        void IMenuHost.IncrementCommandId()
        {
            currentCommandId++;
        }

        void IMenuHost.OnAddMenuItem(string helpText, Menu.ContextCommand contextCommand, CustomMenuHandler customMenuHandler)
        {
            menuItems.Add(new MenuItem(helpText, contextCommand, customMenuHandler));
        }
    }
}
