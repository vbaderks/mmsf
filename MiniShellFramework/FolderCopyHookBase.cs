// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using MiniShellFramework.Interfaces;

namespace MiniShellFramework
{
    /// <summary>
    /// Provide a base class for copy hook shell extensions.
    /// </summary>
    public abstract class FolderCopyHookBase : ICopyHook
    {
#if DEBUG
        private static int nextId;
        private readonly int id = Interlocked.Increment(ref nextId);
#endif

        protected FolderCopyHookBase()
        {
            Debug.WriteLine("FolderCopyHookBase::Constructor (instance={0})", this);
        }

        protected static void ComRegister(Type type, string name, string description)
        {
            // Register the Folder CopyHook COM object as an approved shell extension. Explorer will only execute approved extensions.
            using (var key =
                Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved", true))
            {
                if (key == null)
                {
                    Debug.WriteLine(@"Failed to open registry key Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved");
                    return;
                }
                key.SetValue(type.GUID.ToString("B"), description);
            }

            // Register the CopyHook COM object as a copy hook handler.
            using (var key = Registry.ClassesRoot.CreateSubKey( @"Directory\ShellEx\CopyHookHandlers"))
            {
                if (key == null)
                {
                    Debug.WriteLine(@"Failed to open registry key Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved");
                    return;
                }
                key.SetValue(name, type.GUID.ToString("B"));
            }
        }

        protected static void ComUnregister(Type type)
        {
            // Unregister the Folder CopyHook COM object as an approved shell extension.
            // It is possible to unregister twice, need to be prepared to handle that case.
            using (var key =
                Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved", true))
            {
                if (key != null)
                {
                    key.DeleteValue(type.GUID.ToString("B"));
                }
            }

            using (var key = Registry.ClassesRoot.CreateSubKey(@"Directory\ShellEx\CopyHookHandlers"))
            {
                if (key != null)
                {
                    key.DeleteValue(type.GUID.ToString("B"));
                }
            }
        }

        public uint CopyCallback(IntPtr parent, FileOperation fileOperation, uint flags, string sourceFolder, uint sourceAttributes, string destinationFolder, uint destinationAttributes)
        {
            Debug.WriteLine("FolderCopyHookBase::CopyCallback (id={0}, fileOperation={1}, sourceFolder={2})", id, fileOperation, sourceFolder);

            // Note: FOF_SILENT indicates that the operation should be silent, but the win32 .H file states that confirmation dialogs are still to be shown.
            var owner = new NativeWindow();
            owner.AssignHandle(parent);
            try
            {
                var result = CopyCallbackCore(owner, fileOperation, flags, sourceFolder, sourceAttributes, destinationFolder, destinationAttributes);
                Debug.Assert(result == DialogResult.Cancel || result == DialogResult.No || result == DialogResult.Yes);
                Debug.WriteLine("FolderCopyHookBase::CopyCallback (id={0}, result={1}", id, result);
                return (uint)result;
            }
            finally 
            {
                owner.ReleaseHandle();
            }
        }

        protected abstract DialogResult CopyCallbackCore(IWin32Window parent, FileOperation fileOperation, uint flags, string sourceFolder,
                                              uint sourceAttributes, string destinationFolder, uint destinationAttributes);
    }
}
