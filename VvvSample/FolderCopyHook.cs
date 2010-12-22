// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MiniShellFramework;
using MiniShellFramework.ComTypes;

namespace VvvSample
{
    [ComVisible(true)]                              // Make this .NET class a COM object (ComVisible is false on assembly level).
    [Guid("5070BD33-0BD4-4B4C-B5C6-9E09FCFD6DD2")]  // Explicitly assign a GUID: easier to reference and to debug.
    [ClassInterface(ClassInterfaceType.None)]       // Only the functions from the COM interfaces should be accessible.
    public sealed class FolderCopyHook : FolderCopyHookBase
    {
        private const string RegistryName = "VVV FolderCopyHook (MMSF Sample)";

        [ComRegisterFunction]
        public static void ComRegisterFunction(Type type)
        {
            Contract.Requires(type != null);
            ComRegister(type, RegistryName, "MMSF Sample ShellExtension (FolderCopyHook)");
        }

        [ComUnregisterFunction]
        public static void ComUnregisterFunction(Type type)
        {
            Contract.Requires(type != null);
            ComUnregister(type, RegistryName);
        }

        protected override DialogResult CopyCallbackCore(IWin32Window owner, FileOperation fileOperation, uint flags, string sourceFolder, uint sourceAttributes, string destinationFolder, uint destinationAttributes)
        {
            if (fileOperation == FileOperation.Delete && sourceFolder.Contains("VVV-MMSF"))
            {
                var result = MessageBox.Show(owner,
                                       string.Format(CultureInfo.CurrentCulture, "Are you sure to delete the folder: {0} ?", sourceFolder),
                                       "VVV Question",
                                       MessageBoxButtons.YesNoCancel);
                Contract.Assume(result == DialogResult.Yes || result == DialogResult.No || result == DialogResult.Cancel);
                return result;
            }

            return DialogResult.Yes;
        }
    }
}
