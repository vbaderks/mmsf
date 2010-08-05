// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using MiniShellFramework.ComTypes;

namespace MiniShellFramework
{
    internal interface IMenuHost
    {
        uint GetCommandId();

        void IncrementCommandId();

        void OnAddMenuItem(string helpText, Menu.ContextCommand contextCommand, CustomMenuHandler customMenuHandler);
    }


    /// <summary>
    /// Wrapper class for a Win32 menu
    /// </summary>
    public class Menu
    {
        private IntPtr hmenu;
        private uint indexMenu;
        private uint idCmd;
        private uint idCmdLast;

        private IMenuHost menuHost;

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        /// <param name="hmenu">The hmenu.</param>
        /// <param name="indexMenu">The index menu.</param>
        /// <param name="idCmd">The id CMD.</param>
        /// <param name="idCmdLast">The id CMD last.</param>
        public Menu(IntPtr hmenu, uint indexMenu, uint idCmd, uint idCmdLast)
        {
            this.hmenu = hmenu;
            this.indexMenu = indexMenu;
            this.idCmd = idCmd;
            this.idCmdLast = idCmdLast;
        }

        internal Menu(IntPtr hmenu, uint indexMenu, uint idCmdLast, IMenuHost menuHost)
        {
            this.hmenu = hmenu;
            this.indexMenu = indexMenu;
            this.idCmdLast = idCmdLast;
            this.menuHost = menuHost;
        }

        static IntPtr CreateSubMenu()
        {
            IntPtr hmenu = CreatePopupMenu();
            if (hmenu == null)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            return hmenu;
        }


        /// <summary>
        /// Adds the sub menu.
        /// </summary>
        /// <returns></returns>
        public Menu AddSubMenu(string menuText, string helpText)
        {
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
        /// defines the command.
        /// </summary>
        public delegate void ContextCommand(ref InvokeCommandInfo invokeCommandInfo, IList<string> fileNames);

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="menuText">The text.</param>
        /// <param name="helpText">The help.</param>
        /// <param name="contextCommand">The context command.</param>
        public void AddItem(string menuText, string helpText, ContextCommand contextCommand)
        {
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
            //// TODO;
        }

        private void InsertMenuItem(ref MenuItemInfo menuItemInfo, string helpText, ContextCommand contextCommand, CustomMenuHandler customMenuHandler)
        {
            CheckIdSpace();
            bool result = InsertMenuItem(hmenu, indexMenu, true, ref menuItemInfo);
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
            menuHost.OnAddMenuItem(helpText, contextCommand, customMenuHandler);

            indexMenu++;
            menuHost.IncrementCommandId();
        }

        [DllImport("user32.dll")]
        static extern bool InsertMenuItem(IntPtr menu, uint uItem, bool byPosition, [In] ref MenuItemInfo menuItemInfo);

        [DllImport("user32.dll")]
        private static extern IntPtr CreatePopupMenu();
    }
}
