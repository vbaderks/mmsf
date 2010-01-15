// <copyright company="Victor Derks">
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MiniShellFramework;
using MiniShellFramework.Interfaces;
using System.Diagnostics;
using System.Windows;

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

            if (filenames.Count != 1)
                return; // only add to the context menu when 1 file is selected.

            var smallBitmapHandler = new SmallBitmapCustomMenuHandler("VVV", 0);
            ////CCustomMenuHandlerPtr qsmallbitmaphandler(new CSmallBitmapHandler(IDS_CONTEXTMENU_VVV_SUBMENU, IDB_MENUICON));
            ////CMenu menuVVV = menu.AddSubMenu(IDS_CONTEXTMENU_VVV_SUBMENU_HELP, qsmallbitmaphandler);
            var menuVvv = menu.AddSubMenu(/* "Special commands for VVV files" */);

            menuVvv.AddItem("&Open with notepad", "Open the VVV file with notepad", OnEditWithNotepadCommand);

            var smallBitmapHandler2 = new SmallBitmapCustomMenuHandler("&About MMSF", 0);
            ////CCustomMenuHandlerPtr qsmallbitmaphandler2(new CSmallBitmapHandler(IDS_CONTEXTMENU_ABOUT_MSF, IDB_MENUICON));
            menuVvv.AddItem(smallBitmapHandler2, "Show the version number of the MMSF", OnAboutMmsf);

            // ... optional add more submenu's or more menu items.
        }

        private static void OnEditWithNotepadCommand(ref CMINVOKECOMMANDINFO invokeCommandInfo, IList<string> fileNames)
        {
            Debug.Assert(fileNames.Count == 1, "fileNames.Count == 1");

            //// Use the command line param to pass the exe filename. This causes
            //// Windows to use the path to find notepad.
            Process.Start("notepad.exe", "\"" + fileNames[0] + "\"");
        }

        private static void OnAboutMmsf(ref CMINVOKECOMMANDINFO invokeCommandInfo, IList<string> fileNames)
        {
            var text = string.Format("Build with MSF version {0}.{1}", 1, 0); // TODO: retrieve version info from mmsf assembly.
            MessageBox.Show(text);
        }
    }
}
