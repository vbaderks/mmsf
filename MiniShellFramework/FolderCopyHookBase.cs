// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;
using MiniShellFramework.ComTypes;

namespace MiniShellFramework
{
    /// <summary>
    /// Provide a base class for copy hook shell extensions.
    /// </summary>
    [ComVisible(true)]                        // Make this .NET class visible to ensure derived class can be COM visible.
    [ClassInterface(ClassInterfaceType.None)] // Only the functions from the COM interfaces should be accessible.
    [ContractClass(typeof(FolderCopyHookBaseContract))]
    public abstract class FolderCopyHookBase : ShellExtension, ICopyHook
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FolderCopyHookBase"/> class.
        /// </summary>
        protected FolderCopyHookBase()
        {
            Debug.WriteLine("[{0}] FolderCopyHookBase::Constructor", Id);
        }

        uint ICopyHook.CopyCallback(IntPtr parentWindow, FileOperation fileOperation, uint flags, string source, uint sourceAttributes, string destination, uint destinationAttributes)
        {
            Debug.WriteLine("[{0}] FolderCopyHookBase::CopyCallback (fileOperation={1}, source={2}, destination={3})", Id, fileOperation, source, destination);

            // Note: FOF_SILENT indicates that the operation should be silent, but the win32 .H file states that confirmation dialogs are still to be shown.
            var owner = new NativeWindow();
            owner.AssignHandle(parentWindow);
            try
            {
                var result = CopyCallbackCore(owner, fileOperation, flags, source, sourceAttributes, destination, destinationAttributes);
                Debug.Assert(result == DialogResult.Cancel || result == DialogResult.No || result == DialogResult.Yes);
                Debug.WriteLine("[{0}] FolderCopyHookBase::CopyCallback (result={1})", Id, result);
                return (uint)result;
            }
            finally
            {
                owner.ReleaseHandle();
            }
        }

        /// <summary>
        /// Adds additional info to the registry to allow the shell to discover the oject as shell extension.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        protected static void ComRegister(Type type, string name, string description)
        {
            Contract.Requires(type != null);
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(!string.IsNullOrEmpty(description));

            RegistryExtensions.AddAsApprovedShellExtension(type, description);

            // Register the CopyHook COM object as a copy hook handler.
            var keyName = @"Directory\ShellEx\CopyHookHandlers\" + name;
            using (var key = Registry.ClassesRoot.CreateSubKey(keyName))
            {
                if (key == null)
                    throw new ApplicationException("Failed to open registry key: " + keyName);

                key.SetValue(string.Empty, type.GUID.ToString("B"));
            }
        }

        /// <summary>
        /// Removed the additional info from the registry that allowed the shell to discover the shell extension.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        protected static void ComUnregister(Type type, string name)
        {
            Contract.Requires(type != null);
            Contract.Requires(!string.IsNullOrEmpty(name));

            RegistryExtensions.RemoveAsApprovedShellExtension(type);

            var keyName = @"Directory\ShellEx\CopyHookHandlers\" + name;
            using (var key = Registry.ClassesRoot.OpenSubKey(keyName, true))
            {
                if (key != null)
                {
                    key.DeleteValue(type.GUID.ToString("B"), false);
                }
            }
        }

        /// <summary>
        /// When overridden in a derived class handles the actual copy hook.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="fileOperation">The file operation.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="sourceFolder">The source folder.</param>
        /// <param name="sourceAttributes">The source attributes.</param>
        /// <param name="destinationFolder">The destination folder.</param>
        /// <param name="destinationAttributes">The destination attributes.</param>
        /// <returns></returns>
        protected abstract DialogResult CopyCallbackCore(IWin32Window parent,
                                                         FileOperation fileOperation,
                                                         uint flags,
                                                         string sourceFolder,
                                                         uint sourceAttributes,
                                                         string destinationFolder,
                                                         uint destinationAttributes);
    }

    [ContractClassFor(typeof(FolderCopyHookBase))]
    abstract class FolderCopyHookBaseContract : FolderCopyHookBase
    {
        protected override DialogResult CopyCallbackCore(IWin32Window parent, FileOperation fileOperation, uint flags, string sourceFolder, uint sourceAttributes, string destinationFolder, uint destinationAttributes)
        {
            Contract.Requires(parent != null);
            Contract.Requires(sourceFolder != null);
            Contract.Ensures(Contract.Result<DialogResult>() == DialogResult.Yes ||
                             Contract.Result<DialogResult>() == DialogResult.No ||
                             Contract.Result<DialogResult>() == DialogResult.Cancel);

            return default(DialogResult);
        }
    }
}
