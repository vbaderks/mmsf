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
using Microsoft.Win32;
using MiniShellFramework.ComTypes;

namespace MiniShellFramework
{
    /// <summary>
    /// Base class for Context Menu shell extension handlers.
    /// </summary>
    [ComVisible(true)]                        // Make this .NET class COM visible to ensure derived class can be COM visible.
    [ClassInterface(ClassInterfaceType.None)] // Only the functions from the COM interfaces should be accessible.
    public abstract class ContextMenuBase : IShellExtInit, IContextMenu3, IMenuHost
    {
        private const uint InitializeMenuPopup = 0x117; // WM_INITMENUPOPUP
        private const uint DrawItem = 0x2B; // WM_DRAWITEM
        private const uint MeasureItem = 0x2C; // WM_MEASUREITEM
        private const uint MenuChar = 0x120; // WM_MENUCHAR

        private uint startCommandId;
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

        int IContextMenu.QueryContextMenu(IntPtr menuHandle, uint position, uint firstCommandId, uint lastCommandId, QueryContextMenuOptions flags)
        {
            Debug.WriteLine("{0}.IContextMenu.QueryContextMenu (ContextMenuBase), menuHandle={1}, position={2}, firstCommandId={3}, lastCommandId={4}, flag={5}",
                this, menuHandle, position, firstCommandId, lastCommandId, flags);

            if (flags.HasFlag(QueryContextMenuOptions.DefaultOnly))
                return HResults.Create(Severity.Success, 0); // don't add anything, only default menu items allowed.

            menuItems.Clear();
            currentCommandId = firstCommandId;
            startCommandId = firstCommandId;
            QueryContextMenuCore(new Menu(menuHandle, position, lastCommandId, this), fileNames);
            return HResults.Create(Severity.Success, (ushort)(currentCommandId - firstCommandId));
        }

        void IContextMenu.InvokeCommand(ref InvokeCommandInfo invokeCommandInfo)
        {
            Debug.WriteLine("{0}.IContextMenu.InvokeCommand (ContextMenuBase), Size={1}", this, invokeCommandInfo.Size);

            if (invokeCommandInfo.lpVerb.ToInt32() >> 16 != 0)
                throw new ArgumentException("Verbs not supported");

            menuItems[(ushort)invokeCommandInfo.lpVerb.ToInt32()].Command(ref invokeCommandInfo, fileNames);
        }

        int IContextMenu.GetCommandString(IntPtr commandIdOffset, GetCommandStringOptions flags, int reserved, IntPtr result, int charCount)
        {
            Debug.WriteLine("{0}.IContextMenu.GetCommandString (ContextMenuBase), commandIdOffset={1}, flags={2}, result={3}, charCount={4}",
                this, commandIdOffset, flags, result, charCount);

            switch (flags)
            {
                case GetCommandStringOptions.HelpText:
                    StringToPtr(menuItems[commandIdOffset.ToInt32()].HelpText, result, charCount);
                    return HResults.OK;

                case GetCommandStringOptions.CanonicalVerb:
                case GetCommandStringOptions.CanonicalVerbAnsi:
                    return HResults.ErrorFail;

                default:
                    throw new NotSupportedException();
            }
        }

        int IContextMenu2.QueryContextMenu(IntPtr hmenu, uint indexMenu, uint idCommandFirst, uint idCmdLast, QueryContextMenuOptions flags)
        {
            return ((IContextMenu)this).QueryContextMenu(hmenu, indexMenu, idCommandFirst, idCmdLast, flags);
        }

        void IContextMenu2.InvokeCommand(ref InvokeCommandInfo invokeCommandInfo)
        {
            ((IContextMenu)this).InvokeCommand(ref invokeCommandInfo);
        }

        int IContextMenu2.GetCommandString(IntPtr idCommand, GetCommandStringOptions uflags, int reserved, IntPtr name, int cch)
        {
            return ((IContextMenu)this).GetCommandString(idCommand, uflags, reserved, name, cch);
        }

        void IContextMenu2.HandleMenuMsg(uint uMsg, IntPtr wParam, IntPtr lParam)
        {
            Debug.WriteLine("{0}.IContextMenu2.HandleMenuMsg (ContextMenuBase), uMsg={1}, wParam={2}, lParam={3}", this, uMsg, wParam, lParam);
            ((IContextMenu3)this).HandleMenuMsg2(uMsg, wParam, lParam, IntPtr.Zero);
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

        void IContextMenu3.HandleMenuMsg(uint uMsg, IntPtr wParam, IntPtr lParam)
        {
            Debug.WriteLine("{0}.IContextMenu3.HandleMenuMsg (ContextMenuBase), uMsg={1}, wParam={2}, lParam={3}",
                this, uMsg, wParam, lParam.ToInt32());
            ((IContextMenu3)this).HandleMenuMsg2(uMsg, wParam, lParam, IntPtr.Zero);
        }

        void IContextMenu3.HandleMenuMsg2(uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr result)
        {
            Debug.WriteLine("{0}.IContextMenu3.HandleMenuMsg2 (ContextMenuBase), uMsg={1}, wParam={2}, lParam={3}, result",
                this, uMsg, wParam, lParam, result);

            // Note: The SDK docs tell that this function is only called for WM_MENUCHAR but this is not true (seen on XP sp3).
            //       HandleMenuMsg2 is called also directly for WM_INITMENUPOPUP, etc when the shell detects that IContextMenu3 is supported.
            switch (uMsg)
            {
                case InitializeMenuPopup:
                    Debug.WriteLine("{0}.IContextMenu3.OnInitMenuPopup (ContextMenuBase)", this);
                    OnInitMenuPopup(wParam, (ushort)lParam);
                    break;

                case DrawItem:
                    Debug.WriteLine("{0}.IContextMenu3.OnDrawItem (ContextMenuBase)", this);
                    ////    return static_cast<T*>(this)->OnDrawItem(reinterpret_cast<DRAWITEMSTRUCT*>(lParam));
                    OnDrawItem();
                    break;

                case MeasureItem:
                    Debug.WriteLine("{0}.IContextMenu3.OnMeasureItem (ContextMenuBase)", this);
                    ////    return static_cast<T*>(this)->OnMeasureItem(reinterpret_cast<MEASUREITEMSTRUCT*>(lParam));
                    OnMeasureItem();
                    break;

                case MenuChar:
                    Debug.WriteLine("{0}.IContextMenu3.OnMenuChar (ContextMenuBase)", this);
                    if (result == IntPtr.Zero)
                        throw new InvalidOperationException();

                    //    *plResult = static_cast<T*>(this)->OnMenuChar(reinterpret_cast<HMENU>(lParam), LOWORD(wParam));
                    break;

                default:
                    throw new NotSupportedException();
            }
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
        /// Called when [init menu popup].
        /// </summary>
        /// <param name="menuHandle">The menu handle.</param>
        /// <param name="index">The index.</param>
        protected virtual void OnInitMenuPopup(IntPtr menuHandle, ushort index)
        {
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

        private static void StringToPtr(string text, IntPtr destination, int charCount)
        {
            var array = text.ToCharArray();
            var length = Math.Min(array.Length, charCount - 1);
            Marshal.Copy(array, 0, destination, length);
            Marshal.WriteInt16(destination, 2 * length, 0);
        }

        private void OnDrawItem(/*DRAWITEMSTRUCT* pdrawitem*/)
        {
            ////if (pdrawitem->CtlType != ODT_MENU)
            ////    return E_INVALIDARG;

            ////GetMenuItem(pdrawitem->itemID - m_idCmdFirst).GetCustomMenuHandler().Draw(*pdrawitem);
        }

        private void OnMeasureItem(/*MEASUREITEMSTRUCT* pmeasureitem*/)
        {
            //GetMenuItem(pmeasureitem->itemID - m_idCmdFirst).GetCustomMenuHandler().Measure(*pmeasureitem
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
            private readonly string helpText;
            private readonly Menu.ContextCommand contextcommand;
            private CustomMenuHandler custommenuhandler;

            public MenuItem(string helpText, Menu.ContextCommand contextcommand, CustomMenuHandler custommenuhandler)
            {
                this.helpText = helpText;
                this.contextcommand = contextcommand;
                this.custommenuhandler = custommenuhandler;
            }

            public string HelpText
            {
                get { return helpText; }
            }

            public Menu.ContextCommand Command
            {
                get { return contextcommand; }
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
