// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using MiniShellFramework;
using MiniShellFramework.ComTypes;

namespace VvvSample
{
    [ComVisible(true)]                              // Make this .NET class a COM object (ComVisible is false on assembly level).
    [Guid("1B40F482-7ECC-48C2-BA0A-D7D940DAA7AA")]  // Explicitly assign a GUID: easier to reference and to debug.
    [ClassInterface(ClassInterfaceType.None)]       // Only the functions from the COM interfaces should be accessible.
    public sealed class ContextMenu : ContextMenuBase, IDisposable
    {
        private readonly Bitmap bitmapMenuIcon;

        public ContextMenu()
        {
            bitmapMenuIcon = new Bitmap(typeof(ContextMenu), "menuicon.bmp");
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

        protected override void QueryContextMenuCore(Menu menu, IList<string> filenames, QueryContextMenuOptions flags)
        {
            if (filenames.Count != 1 || ContainsUnknownExtension(filenames))
                return; // In this sample only extend the menu when only 1 .mvvv file is selected.

            var menuMvvv = menu.AddSubMenu("Special commands for VVV files", new SmallBitmapCustomMenuHandler("MVVV", bitmapMenuIcon));
            menuMvvv.AddItem("&Open with notepad", "Open the VVV file with notepad", OnEditWithNotepadCommand);
            menuMvvv.AddItem(new SmallBitmapCustomMenuHandler("&About MMSF", bitmapMenuIcon), "Show the version number of the MMSF", OnAboutMmsf);

            // ... optional add more submenu's or more menu items.
        }

        private static void OnEditWithNotepadCommand(ref InvokeCommandInfo invokeCommandInfo, IList<string> fileNames)
        {
            // Use the command line parameters to pass the exe filename. This causes
            // Windows to use the path to find notepad.
            Process.Start("notepad.exe", "\"" + fileNames[0] + "\"");
        }

        private static void OnAboutMmsf(ref InvokeCommandInfo invokeCommandInfo, IList<string> fileNames)
        {
            var fvi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            var text = $"Build with MSF version {fvi.ProductVersion}";
            System.Windows.Forms.MessageBox.Show(text);
        }

        public void Dispose()
        {
            Debug.WriteLine("{0}.Dispose", this);
            bitmapMenuIcon.Dispose();
        }
    }
}
