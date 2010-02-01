// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Windows;
using Microsoft.Win32;
using MiniShellFramework.Interfaces;

namespace MiniShellFramework
{
    public abstract class CopyHookBase : ICopyHook
    {
        public static void ComRegisterFunction(Type type, string name, string description)
        {
            // Register the CopyHook COM object as an approved shell extension. Explorer will only execute approved extensions.
            using (var key =
                Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved", true))
            {
                key.SetValue(type.GUID.ToString("B"), description);
            }

            // Register the CopyHook COM object as a copy hook handler.
            using (var key = Registry.ClassesRoot.CreateSubKey( @"Directory\ShellEx\CopyHookHandlers"))
            {
                key.SetValue(name, type.GUID.ToString("B"));
            }
        }

        public void CopyCallback(IntPtr hwnd, FileOperation fileOperation, uint flags, string sourceFile, uint sourceAttributes, string destinationFile, uint destinationAttributes)
        {
            CopyCallbackCore(hwnd, fileOperation, flags, sourceFile, sourceAttributes, destinationFile, destinationAttributes);
        }

        public abstract MessageBoxResult CopyCallbackCore(IntPtr hwnd, FileOperation fileOperation, uint flags, string sourceFile,
                                              uint sourceAttributes, string destinationFile, uint destinationAttributes);
    }
}
