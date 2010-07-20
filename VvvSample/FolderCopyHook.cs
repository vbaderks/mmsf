// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MiniShellFramework;
using MiniShellFramework.Interfaces;

namespace VvvSample
{
    [ComVisible(true)]                              // Make this .NET class a COM object (ComVisible is false on assembly level).
    [Guid("B7096869-8E27-4f13-A9B9-3164F6D30BAB")]  // Explicitly assign a GUID: easier to reference and to debug.
    [ClassInterface(ClassInterfaceType.None)]       // Only the functions from the COM interfaces should be accessible.
    public class FolderCopyHook : FolderCopyHookBase
    {
        [ComRegisterFunction]
        public static void ComRegisterFunction(Type type)
        {
            ComRegister(type, "VVV FolderCopyHook", "VVV Sample ShellExtension (FolderCopyHook)");
        }

        [ComUnregisterFunction]
        public static void ComUnregisterFunction(Type type)
        {
            ComUnregister(type);
        }

        protected override DialogResult CopyCallbackCore(IWin32Window owner, FileOperation fileOperation, uint flags, string sourceFile, uint sourceAttributes, string destinationFile, uint destinationAttributes)
        {
            if (fileOperation == FileOperation.Delete && sourceFile.Contains("VVV"))
            {
                return MessageBox.Show(owner, "are you sure to delete this folder?", "VVV Question",
                                MessageBoxButtons.YesNoCancel);
            }

            return DialogResult.Yes;
        }
    }
}
