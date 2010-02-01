// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Runtime.InteropServices;
using System.Windows;
using MiniShellFramework;
using MiniShellFramework.Interfaces;

namespace VvvSample
{
    [ComVisible(true)]                              // Make this .NET class a COM object (ComVisible is false on assembly level).
    [Guid("B7096869-8E27-4f13-A9B9-3164F6D30BAB")]  // Explicitly assign a GUID: easier to reference and to debug.
    [ClassInterface(ClassInterfaceType.None)]       // Only the functions from the COM interfaces should be accessible.
    public class CopyHook : CopyHookBase
    {
        [ComRegisterFunction]
        public static void ComRegisterFunction(Type type)
        {
            VvvRootKey.Register();
            ComRegisterFunction(type, "VVV CopyHook", "VVV Sample ShellExtension (CopyHook)");
        }

        public override MessageBoxResult CopyCallbackCore(IntPtr hwnd, FileOperation fileOperation, uint flags, string sourceFile, uint sourceAttributes, string destinationFile, uint destinationAttributes)
        {
            if (fileOperation == FileOperation.Delete && sourceFile.Contains("VVV"))
            {
                MessageBox.Show("TODO");
                //    return IsolationAwareMessageBox(hwnd, LoadString(IDS_COPYHOOK_QUESTION),
                //        LoadString(IDS_COPYHOOK_CAPTION), MB_YESNOCANCEL);
            }

            return MessageBoxResult.Yes;
        }
    }
}
