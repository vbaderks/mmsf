using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

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

        public void AddSubMenu()
        {
        }

        void AddItem(string text, string help /*, CContextCommandPtr qcontextcommand */)
        {
            var menuItemInfo = new MENUITEMINFO();
            MENUITEMINFO.Initialize(ref menuItemInfo);
            menuItemInfo.Id = idCmd;
            menuItemInfo.Text = text;

            ////CCustomMenuHandlerPtr qcustommenuhandler;
            InsertMenuItem(ref menuItemInfo, help /*, qcontextcommand, qcustommenuhandler*/);
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
