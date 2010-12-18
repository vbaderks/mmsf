// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using MiniShellFramework.ComTypes;

namespace MiniShellFramework
{
    /// <summary>
    /// Common extensions to add keys to the registry for shell extensions.
    /// </summary>
    public static class RegistryExtensions
    {
        /// <summary>
        /// Adds as approved shell extension.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="description">The description.</param>
        public static void AddAsApprovedShellExtension(Type type, string description)
        {
            Contract.Requires(type != null);
            Contract.Requires(!string.IsNullOrEmpty(description));

            // Register the Folder CopyHook COM object as an approved shell extension. Explorer will only execute approved extensions.
            using (var key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved", true))
            {
                if (key == null)
                    throw new ApplicationException(
                            @"Failed to open registry key Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved");

                key.SetValue(type.GUID.ToString("B"), description);
            }
        }

        /// <summary>
        /// Removes as approved shell extension.
        /// </summary>
        /// <param name="type">The type.</param>
        public static void RemoveAsApprovedShellExtension(Type type)
        {
            Contract.Requires(type != null);

            // Unregister the Folder CopyHook COM object as an approved shell extension.
            // It is possible to unregister twice, need to be prepared to handle that case.
            using (var key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved", true))
            {
                if (key != null)
                {
                    key.DeleteValue(type.GUID.ToString("B"));
                }
            }
        }
    }



    /// <summary>
    /// Provide a base class for copy hook shell extensions.
    /// </summary>
    [ComVisible(true)]                        // Make this .NET class visible to ensure derived class can be COM visible.
    [ClassInterface(ClassInterfaceType.None)] // Only the functions from the COM interfaces should be accessible.
    [ContractClass(typeof(FolderCopyHookBaseContract))]
    public abstract class FolderCopyHookBase : ICopyHook
    {
#if DEBUG
        private readonly int id = Interlocked.Increment(ref nextId);
        private static int nextId;
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderCopyHookBase"/> class.
        /// </summary>
        protected FolderCopyHookBase()
        {
            Debug.WriteLine("FolderCopyHookBase::Constructor (instance={0})", this);
        }

        uint ICopyHook.CopyCallback(IntPtr parentWindow, FileOperation fileOperation, uint flags, string source, uint sourceAttributes, string destination, uint destinationAttributes)
        {
            Debug.WriteLine("FolderCopyHookBase::CopyCallback (id={0}, fileOperation={1}, source={2}, destination={3})", id, fileOperation, source, destination);

            // Note: FOF_SILENT indicates that the operation should be silent, but the win32 .H file states that confirmation dialogs are still to be shown.
            var owner = new NativeWindow();
            owner.AssignHandle(parentWindow);
            try
            {
                var result = CopyCallbackCore(owner, fileOperation, flags, source, sourceAttributes, destination, destinationAttributes);
                Debug.Assert(result == DialogResult.Cancel || result == DialogResult.No || result == DialogResult.Yes);
                Debug.WriteLine("FolderCopyHookBase::CopyCallback (id={0}, result={1})", id, result);
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
            using (var key = Registry.ClassesRoot.OpenSubKey(keyName))
            {
                if (key != null)
                {
                    key.DeleteValue(type.GUID.ToString("B"));
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
