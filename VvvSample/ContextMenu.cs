// <copyright company="Victor Derks">
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using MiniShellFramework;

namespace VvvSample
{
    [ComVisible(true)]                              // Make this .NET class a COM object (ComVisible is false on assembly level).
    [Guid("B498A476-9EB6-46c3-8146-CE77FF7EA063")]  // Explicitly assign a GUID: easier to reference and to debug.
    [ClassInterface(ClassInterfaceType.None)]       // Only the functions from the COM interfaces should be accessible.
    public class ContextMenu : ContextMenuBase
    {
        [ComRegisterFunction]
        public static void ComRegisterFunction(Type type)
        {
            VvvRootKey.Register();
            ComRegisterFunction(type, "VVV Sample ShellExtension (ContextMenu)", VvvRootKey.ProgId);
        }

        [ComUnregisterFunction]
        public static void ComUnregisterFunction(Type type)
        {
            VvvRootKey.Unregister();
            ComUnregisterFunction(type);
        }

        protected override void QueryContextMenuCore(Menu menu, IList<string> filenames)
        {
            ////if (ContainsUnknownExtension(filenames))
            ////    return; // only extend the menu when only .vvv files are selected.

            ////if (filenames.size() != 1)
            ////    return; // only add to the context menu when 1 file is selected.

            ////CCustomMenuHandlerPtr qsmallbitmaphandler(new CSmallBitmapHandler(IDS_CONTEXTMENU_VVV_SUBMENU, IDB_MENUICON));
            ////CMenu menuVVV = menu.AddSubMenu(IDS_CONTEXTMENU_VVV_SUBMENU_HELP, qsmallbitmaphandler);

            ////CContextCommandPtr qeditwithnotepadcommand(new CEditWithNotepadCommand());
            ////menuVVV.AddItem(IDS_CONTEXTMENU_EDIT_WITH_NOTEPAD,
            ////                IDS_CONTEXTMENU_EDIT_WITH_NOTEPAD_HELP, qeditwithnotepadcommand);

            ////CContextCommandPtr qaboutmsfcommand(new CAboutMSFCommand());
            ////CCustomMenuHandlerPtr qsmallbitmaphandler2(new CSmallBitmapHandler(IDS_CONTEXTMENU_ABOUT_MSF, IDB_MENUICON));
            ////menuVVV.AddItem(IDS_CONTEXTMENU_ABOUT_MSF_HELP, qaboutmsfcommand, qsmallbitmaphandler2);

            // ... optional add more submenu's or more menu items.
        }
    }
}
