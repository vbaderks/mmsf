// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.InteropServices;
using MiniShellFramework.ComTypes;

namespace MiniShellFramework
{
    /// <summary>
    /// Wrapper class for a Win32 menu
    /// </summary>
    public class Menu
    {
        private readonly IntPtr hmenu;
        private readonly uint idCmdLast;
        private readonly IMenuHost menuHost;
        private uint indexMenu;
        ////private uint idCmd;

        ////public Menu(IntPtr hmenu, uint indexMenu, uint idCmd, uint idCmdLast)
        ////{
        ////    this.hmenu = hmenu;
        ////    this.indexMenu = indexMenu;
        ////    this.idCmd = idCmd;
        ////    this.idCmdLast = idCmdLast;
        ////}

        internal Menu(IntPtr hmenu, uint indexMenu, uint idCmdLast, IMenuHost menuHost)
        {
            Contract.Requires(hmenu != null);
            Contract.Requires(menuHost != null);

            this.hmenu = hmenu;
            this.indexMenu = indexMenu;
            this.idCmdLast = idCmdLast;
            this.menuHost = menuHost;
        }

        /// <summary>
        /// defines the command.
        /// </summary>
        public delegate void ContextCommand(ref InvokeCommandInfo invokeCommandInfo, IList<string> fileNames);

        /// <summary>
        /// Adds the sub menu.
        /// </summary>
        /// <returns></returns>
        public Menu AddSubMenu(string menuText, string helpText)
        {
            Contract.Requires(helpText != null);
            Contract.Ensures(Contract.Result<Menu>() != null);

            IntPtr subMenu = CreateSubMenu();

            var menuItemInfo = new MenuItemInfo();
            menuItemInfo.InitializeSize();
            menuItemInfo.Id = menuHost.GetCommandId();
            menuItemInfo.Text = menuText;
            menuItemInfo.SubMenu = subMenu;

            InsertMenuItem(ref menuItemInfo, helpText, null, null);

            return new Menu(subMenu, indexMenu, idCmdLast, menuHost);
        }

        /// <summary>
        /// Adds the sub menu.
        /// </summary>
        /// <returns></returns>
        public Menu AddSubMenu(string helpText, CustomMenuHandler customMenuHandler)
        {
            Contract.Requires(helpText != null);
            Contract.Requires(customMenuHandler != null);

            IntPtr subMenu = CreateSubMenu();

            var menuItemInfo = new MenuItemInfo();
            menuItemInfo.InitializeSize();
            menuItemInfo.Id = menuHost.GetCommandId();
            menuItemInfo.SubMenu = subMenu;

            customMenuHandler.InitializeItemInfo(ref menuItemInfo);

            InsertMenuItem(ref menuItemInfo, helpText, null, customMenuHandler);

            return new Menu(subMenu, indexMenu, idCmdLast, menuHost);
        }

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="menuText">The text.</param>
        /// <param name="helpText">The help.</param>
        /// <param name="contextCommand">The context command.</param>
        public void AddItem(string menuText, string helpText, ContextCommand contextCommand)
        {
            Contract.Requires(helpText != null);
            Contract.Requires(contextCommand != null);

            var menuItemInfo = new MenuItemInfo();
            menuItemInfo.InitializeSize();
            menuItemInfo.Id = menuHost.GetCommandId();
            menuItemInfo.Text = menuText;

            InsertMenuItem(ref menuItemInfo, helpText, contextCommand, null);
        }

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="customMenuHandler">The custom menu handler.</param>
        /// <param name="helpText">The help.</param>
        /// <param name="contextCommand">The context command.</param>
        public void AddItem(CustomMenuHandler customMenuHandler, string helpText, ContextCommand contextCommand)
        {
            Contract.Requires(customMenuHandler != null);
            Contract.Requires(helpText != null);
            Contract.Requires(contextCommand != null);

            var menuItemInfo = new MenuItemInfo();
            menuItemInfo.InitializeSize();
            menuItemInfo.Id = menuHost.GetCommandId();

            customMenuHandler.InitializeItemInfo(ref menuItemInfo);

            InsertMenuItem(ref menuItemInfo, helpText, contextCommand, customMenuHandler);
        }

        internal static IntPtr CreateSubMenu()
        {
            IntPtr hmenu = SafeNativeMethods.CreatePopupMenu();
            if (hmenu == null)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            return hmenu;
        }

        private void InsertMenuItem(ref MenuItemInfo menuItemInfo, string helpText, ContextCommand contextCommand, CustomMenuHandler customMenuHandler)
        {
            Contract.Requires(helpText != null);

            CheckIdSpace();
            bool result = SafeNativeMethods.InsertMenuItem(hmenu, indexMenu, true, ref menuItemInfo);
            if (!result)
                throw new Win32Exception();

            PostAddItem(helpText, contextCommand, customMenuHandler);
        }

        private void CheckIdSpace()
        {
            if (menuHost.GetCommandId() >= idCmdLast)
                throw new COMException(
                        string.Format(
                                CultureInfo.InvariantCulture,
                                "Out of id space (idCmd={0}, idCmdLast={1})",
                                menuHost.GetCommandId(),
                                idCmdLast));
        }

        private void PostAddItem(string helpText, ContextCommand contextCommand, CustomMenuHandler customMenuHandler)
        {
            Contract.Requires(helpText != null);

            menuHost.OnAddMenuItem(helpText, contextCommand, customMenuHandler);

            indexMenu++;
            menuHost.IncrementCommandId();
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(menuHost != null);
        }
    }
}
