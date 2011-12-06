// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using MiniShellFramework.ComTypes;

namespace MiniShellFramework
{
    /// <summary>
    /// Base class for Context Menu shell extension handlers.
    /// </summary>
    [ComVisible(true)]                        // Make this .NET class COM visible to ensure derived class can be COM visible.
    [ClassInterface(ClassInterfaceType.None)] // Only the functions from the COM interfaces should be accessible.
    public abstract class ContextMenuBase : ShellExtensionInit, IContextMenu3, IMenuHost
    {
        private const uint InitializeMenuPopup = 0x117; // WM_INITMENUPOPUP
        private const uint DrawItem = 0x2B;             // WM_DRAWITEM
        private const uint MeasureItem = 0x2C;          // WM_MEASUREITEM
        private const uint MenuChar = 0x120;            // WM_MENUCHAR

        private uint startCommandId;
        private uint currentCommandId;
        private readonly List<MenuItem> menuItems = new List<MenuItem>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextMenuBase"/> class.
        /// </summary>
        protected ContextMenuBase()
        {
            Debug.WriteLine("[{0}] ContextMenuBase.Constructor", Id);
        }

        int IContextMenu.QueryContextMenu(IntPtr menuHandle, uint position, uint firstCommandId, uint lastCommandId, QueryContextMenuOptions flags)
        {
            Debug.WriteLine("[{0}] ContextMenuBase.IContextMenu.QueryContextMenu (menuHandle={1}, position={2}, firstCommandId={3}, lastCommandId={4}, flag={5})",
                Id, menuHandle, position, firstCommandId, lastCommandId, flags);

            if (flags.HasFlag(QueryContextMenuOptions.DefaultOnly))
                return HResults.Create(Severity.Success, 0); // don't add anything, only default menu items allowed.

            menuItems.Clear();
            currentCommandId = firstCommandId;
            startCommandId = firstCommandId;
            QueryContextMenuCore(new Menu(menuHandle, position, lastCommandId, this), FilesNames);
            return HResults.Create(Severity.Success, (ushort)(currentCommandId - firstCommandId));
        }

        void IContextMenu.InvokeCommand(ref InvokeCommandInfo invokeCommandInfo)
        {
            Debug.WriteLine("[{0}] ContextMenuBase.IContextMenu.InvokeCommand (Size={1})", Id, invokeCommandInfo.Size);

            if (invokeCommandInfo.lpVerb.ToInt32() >> 16 != 0)
                throw new ArgumentException("Verbs not supported");

            var index = (ushort)invokeCommandInfo.lpVerb.ToInt32();
            Contract.Assume(index < menuItems.Count);
            var item = menuItems[index];
            Contract.Assume(item != null);
            Contract.Assume(item.Command != null);
            item.Command(ref invokeCommandInfo, FilesNames);
        }

        int IContextMenu.GetCommandString(IntPtr commandIdOffset, GetCommandStringOptions flags, int reserved, IntPtr result, int charCount)
        {
            Debug.WriteLine("[{0}] ContextMenuBase.IContextMenu.GetCommandString (commandIdOffset={1}, flags={2}, result={3}, charCount={4})",
                Id, commandIdOffset, flags, result, charCount);

            switch (flags)
            {
                case GetCommandStringOptions.HelpText:
                    var index = commandIdOffset.ToInt32();
                    Contract.Assume(index >= 0 && index < menuItems.Count);
                    var item = menuItems[index];
                    Contract.Assume(item != null);
                    StringToPtr(item.HelpText, result, charCount);
                    return HResults.Ok;

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
            Debug.WriteLine("[{0}] ContextMenuBase.IContextMenu2.HandleMenuMsg (uMsg={1}, wParam={2}, lParam={3})", Id, uMsg, wParam, lParam);
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
            Debug.WriteLine("[{0}] ContextMenuBase.IContextMenu3.HandleMenuMsg (uMsg={1}, wParam={2}, lParam={3})",
                Id, uMsg, wParam, lParam.ToInt32());
            ((IContextMenu3)this).HandleMenuMsg2(uMsg, wParam, lParam, IntPtr.Zero);
        }

        unsafe void IContextMenu3.HandleMenuMsg2(uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr result)
        {
            Debug.WriteLine("[{0}] ContextMenuBase.IContextMenu3.HandleMenuMsg2 (uMsg={1}, wParam={2}, lParam={3}, result={4})",
                Id, uMsg, wParam, lParam, result);

            // The SDK docs tell that this function is only called for WM_MENUCHAR but this is not true (seen on XP sp3).
            // HandleMenuMsg2 is called also directly for WM_INITMENUPOPUP, etc when the shell detects that IContextMenu3 is supported.
            switch (uMsg)
            {
                case InitializeMenuPopup:
                    Debug.WriteLine("[{0}] ContextMenuBase.IContextMenu3.OnInitMenuPopup", Id);
                    OnInitMenuPopup(wParam, (ushort)lParam);
                    break;

                case DrawItem:
                    Debug.WriteLine("[{0}] ContextMenuBase.IContextMenu3.OnDrawItem", Id);
                    OnDrawItem(ref *(DrawItem*)lParam.ToPointer());
                    break;

                case MeasureItem:
                    Debug.WriteLine("[{0}] ContextMenuBase.IContextMenu3.OnMeasureItem", Id);
                    OnMeasureItem(ref *(MEASUREITEMSTRUCT*)lParam.ToPointer());
                    break;

                case MenuChar:
                    Debug.WriteLine("[{0}] ContextMenuBase.IContextMenu3.OnMenuChar", Id);
                    if (result == IntPtr.Zero)
                        throw new InvalidOperationException();

                    var pointerResult = (int*)result.ToPointer();
                    *pointerResult = OnMenuChar(lParam, (ushort)wParam.ToInt32());
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
            Contract.Requires(type != null);
            Contract.Requires(!string.IsNullOrEmpty(description));
            Contract.Requires(!string.IsNullOrEmpty(progId));

            RegistryExtensions.AddAsApprovedShellExtension(type, description);

            // Register the ContextMenu COM object as the ContextMenu handler.
            var subKeyName = progId + @"\ShellEx\ContextMenuHandlers\" + description;
            using (var key = Registry.ClassesRoot.CreateSubKey(subKeyName))
            {
                if (key == null)
                    throw new ApplicationException("Failed to create sub key: " + subKeyName);

                key.SetValue(string.Empty, type.GUID.ToString("B"));
            }
        }

        /// <summary>
        /// Removed the additional info from the registry that allowed the shell to discover the shell extension.
        /// </summary>
        /// <remarks>
        /// This function will only remove the COM registration from the specific ContextMenu. To prevent breaking
        /// other COM registrations other shell extensions are left untouched.
        /// It may be required to also remove the ProgID, this should be done in a separate method.
        /// </remarks>
        /// <param name="type">The type.</param>
        /// <param name="description">The description.</param>
        /// <param name="progId">The prog id.</param>
        protected static void ComUnregister(Type type, string description, string progId)
        {
            Contract.Requires(type != null);
            Contract.Requires(!string.IsNullOrEmpty(description));
            Contract.Requires(!string.IsNullOrEmpty(progId));

            // Remove the ContextMenu COM registration.
            // Leave the 'ContextMenuHandlers' subkey intact, other handlers may also be installed.
            using (var contextMenuHandlersKey = Registry.ClassesRoot.OpenSubKey(progId + @"\ShellEx\ContextMenuHandlers", true))
            {
                if (contextMenuHandlersKey != null)
                {
                    contextMenuHandlersKey.DeleteSubKey(description, false);
                }
            }

            RegistryExtensions.RemoveAsApprovedShellExtension(type);
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

        private static void StringToPtr(string text, IntPtr destination, int charCount)
        {
            Contract.Requires(text != null);

            var array = text.ToCharArray();
            var length = Math.Min(array.Length, charCount - 1);
            Marshal.Copy(array, 0, destination, length);
            Marshal.WriteInt16(destination, 2 * length, 0);
        }

        private void OnDrawItem(ref DrawItem drawItem)
        {
            if (drawItem.CtlType != OwnerDrawControlType.Menu)
                throw new COMException("CtlType is not menu");

            GetMenuItem((int)(drawItem.itemID - startCommandId)).CustomMenuHandler.Draw(ref drawItem);
        }

        private void OnMeasureItem(ref MEASUREITEMSTRUCT measureItem)
        {
            GetMenuItem((int)(measureItem.itemID - startCommandId)).CustomMenuHandler.Measure(ref measureItem);
        }

        private int OnMenuChar(IntPtr handleMenu, ushort character)
        {
            ushort result = CustomMenuHandler.MenuCharResultIgnore;

            foreach (var item in menuItems)
            {
                if (item.CustomMenuHandler != null)
                {
                    if (item.CustomMenuHandler.OnMenuChar(handleMenu, character, out result))
                        break;
                }
            }

            return (0 << 16) | result;
        }

        private MenuItem GetMenuItem(int index)
        {
            return menuItems[index];
        }

        private class MenuItem
        {
            private readonly string helpText;
            private readonly Menu.ContextCommand contextcommand;
            private readonly CustomMenuHandler custommenuhandler;

            public MenuItem(string helpText, Menu.ContextCommand contextcommand, CustomMenuHandler custommenuhandler)
            {
                Contract.Requires(helpText != null);

                this.helpText = helpText;
                this.contextcommand = contextcommand;
                this.custommenuhandler = custommenuhandler;
            }

            public string HelpText
            {
                get
                {
                    Contract.Ensures(Contract.Result<string>() != null);
                    return helpText;
                }
            }

            public Menu.ContextCommand Command
            {
                get
                {
                    return contextcommand;
                }
            }

            public CustomMenuHandler CustomMenuHandler
            {
                get
                {
                    return custommenuhandler;
                }
            }

            [ContractInvariantMethod]
            private void ObjectInvariant()
            {
                Contract.Invariant(helpText != null);
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

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(menuItems != null);
        }
    }
}
