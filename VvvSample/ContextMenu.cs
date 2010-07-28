// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using MiniShellFramework;
using MiniShellFramework.ComTypes;

namespace VvvSample
{
    [ComVisible(true)]                              // Make this .NET class a COM object (ComVisible is false on assembly level).
    [Guid("1B40F482-7ECC-48C2-BA0A-D7D940DAA7AA")]  // Explicitly assign a GUID: easier to reference and to debug.
    [ClassInterface(ClassInterfaceType.None)]       // Only the functions from the COM interfaces should be accessible.
    public class ContextMenu : ContextMenuBase
    {
        public ContextMenu()
        {
            RegisterExtension(".mvvv");
        }

        [ComRegisterFunction]
        public static void ComRegisterFunction(Type type)
        {
            VvvRootKey.Register();
            ComRegister(type, "VVV ContextMenu (MMSF Sample)", VvvRootKey.ProgId);
        }

        [ComUnregisterFunction]
        public static void ComUnregisterFunction(Type type)
        {
            VvvRootKey.Unregister();
            ComUnregister(type, "VVV ContextMenu (MMSF Sample)", VvvRootKey.ProgId);
        }

        protected override void QueryContextMenuCore(Menu menu, IList<string> filenames)
        {
            if (ContainsUnknownExtension(filenames))
                return; // only extend the menu when only .mvvv files are selected.

            if (filenames.Count != 1)
                return; // only add to the context menu when 1 file is selected.

            ////CCustomMenuHandlerPtr qsmallbitmaphandler(new CSmallBitmapHandler(IDS_CONTEXTMENU_VVV_SUBMENU, IDB_MENUICON));
            var smallBitmapHandler = new SmallBitmapCustomMenuHandler("MVVV", 0);

            ////CMenu menuVVV = menu.AddSubMenu(IDS_CONTEXTMENU_VVV_SUBMENU_HELP, qsmallbitmaphandler);
            var menuVvv = menu.AddSubMenu("Special commands for VVV files", smallBitmapHandler);

            menuVvv.AddItem("&Open with notepad", "Open the VVV file with notepad", OnEditWithNotepadCommand);

            ////CCustomMenuHandlerPtr qsmallbitmaphandler2(new CSmallBitmapHandler(IDS_CONTEXTMENU_ABOUT_MSF, IDB_MENUICON));
            var smallBitmapHandler2 = new SmallBitmapCustomMenuHandler("&About MMSF", 0);
            menuVvv.AddItem(smallBitmapHandler2, "Show the version number of the MMSF", OnAboutMmsf);

            // ... optional add more submenu's or more menu items.
        }

        private static void OnEditWithNotepadCommand(ref InvokeCommandInfo invokeCommandInfo, IList<string> fileNames)
        {
            Debug.Assert(fileNames.Count == 1, "fileNames.Count == 1");

            //// Use the command line param to pass the exe filename. This causes
            //// Windows to use the path to find notepad.
            Process.Start("notepad.exe", "\"" + fileNames[0] + "\"");
        }

        private static void OnAboutMmsf(ref InvokeCommandInfo invokeCommandInfo, IList<string> fileNames)
        {
            var text = string.Format("Build with MSF version {0}.{1}", 1, 0); // TODO: retrieve version info from mmsf assembly.
            MessageBox.Show(text);
        }
    }
}
