// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MiniShellFramework.ComTypes;

namespace MiniShellFramework
{
    public class Menu
    {
        private IntPtr hmenu;
        private uint indexMenu;
        private uint idCmd;
        private uint idCmdLast;

        public Menu(IntPtr hmenu, uint indexMenu, uint idCmd, uint idCmdLast)
        {
            this.hmenu = hmenu;
            this.indexMenu = indexMenu;
            this.idCmd = idCmd;
            this.idCmdLast = idCmdLast;
        }

        public Menu AddSubMenu()
        {
            return null;
        }

        public delegate void ContextCommand(ref CMINVOKECOMMANDINFO invokeCommandInfo, IList<string> fileNames);

        public void AddItem(string text, string help, ContextCommand contextCommand)
        {
            var menuItemInfo = new MENUITEMINFO();
            MENUITEMINFO.Initialize(ref menuItemInfo);
            menuItemInfo.Id = idCmd;
            menuItemInfo.Text = text;

            ////CCustomMenuHandlerPtr qcustommenuhandler;
            InsertMenuItem(ref menuItemInfo, help /*, qcontextcommand, qcustommenuhandler*/);
        }

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
