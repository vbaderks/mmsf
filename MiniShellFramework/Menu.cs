// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MiniShellFramework.ComTypes;

namespace MiniShellFramework
{
    /// <summary>
    /// Wrapper class for a Win32 menu
    /// </summary>
    public class Menu
    {
        private IntPtr hmenu;
        private uint indexMenu;
        private uint idCmd;
        private uint idCmdLast;

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

        /// <summary>
        /// Adds the sub menu.
        /// </summary>
        /// <returns></returns>
        public Menu AddSubMenu()
        {
            return null;
        }

        /// <summary>
        /// defines the command.
        /// </summary>
        public delegate void ContextCommand(ref InvokeCommandInfo invokeCommandInfo, IList<string> fileNames);

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="help">The help.</param>
        /// <param name="contextCommand">The context command.</param>
        public void AddItem(string text, string help, ContextCommand contextCommand)
        {
            var menuItemInfo = new MENUITEMINFO();
            MENUITEMINFO.Initialize(ref menuItemInfo);
            menuItemInfo.Id = idCmd;
            menuItemInfo.Text = text;

            ////CCustomMenuHandlerPtr qcustommenuhandler;
            InsertMenuItem(ref menuItemInfo, help /*, qcontextcommand, qcustommenuhandler*/);
        }

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="customMenuHandler">The custom menu handler.</param>
        /// <param name="help">The help.</param>
        /// <param name="contextCommand">The context command.</param>
        public void AddItem(CustomMenuHandler customMenuHandler, string help, ContextCommand contextCommand)
        {
            //// TODO;
        }

        private void InsertMenuItem(ref MENUITEMINFO menuItemInfo, string help)
        {
            bool result = InsertMenuItem(hmenu, indexMenu, true, ref menuItemInfo);
            // TODO: throw if false;
        }

        ////void InsertMenuItem(const CMenuItemInfo& menuiteminfo,
        ////                    const CString& strHelp,
        ////                    CContextCommandPtr qcontextcommand,
        ////                    CCustomMenuHandlerPtr qcustommenuhandler)
        ////{
        ////    CheckID();

        ////    RaiseLastErrorExceptionIf(
        ////        !::InsertMenuItem(_hmenu, _indexMenu, true, &menuiteminfo));

        ////    PostAddItem(strHelp, qcontextcommand, qcustommenuhandler);
        ////}

        [DllImport("user32.dll")]
        static extern bool InsertMenuItem(IntPtr hMenu, uint uItem, bool fByPosition, [In] ref MENUITEMINFO lpmii);
    }
}
