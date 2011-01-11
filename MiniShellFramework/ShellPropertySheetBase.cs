// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using MiniShellFramework.ComTypes;

namespace MiniShellFramework
{
    /// <summary>
    /// Provide a base class for property sheet shell extensions.
    /// </summary>
    [ComVisible(true)]                        // Make this .NET class visible to ensure derived class can be COM visible.
    [ClassInterface(ClassInterfaceType.None)] // Only the functions from the COM interfaces should be accessible.
    public abstract class ShellPropertySheetBase : ShellExtensionInit, IShellPropSheetExt
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellPropertySheetBase"/> class.
        /// </summary>
        protected ShellPropertySheetBase()
        {
            Debug.WriteLine("[{0}] ShellPropertySheetBase.Constructor ()", Id);
        }

        int IShellPropSheetExt.AddPages(IntPtr addPageFunction, IntPtr lParam)
        {
            Debug.WriteLine("[{0}] ShellPropertySheetBase.IShellPropSheetExt.AddPages (lParam={1})", Id, lParam);

            var d = Marshal.GetDelegateForFunctionPointer(addPageFunction, typeof(AddPropertySheetPage));

            return 0; // TODO: return index for page to display.
        }

        void IShellPropSheetExt.ReplacePage(uint pageId, IntPtr replaceWithFunction, IntPtr lParam)
        {
            Debug.WriteLine("[{0}] ShellPropertySheetBase.IShellPropSheetExt.ReplacePage - Not Implemented (functionality not used)", Id);

            // The Shell doesn't call this function for file class Property Sheets.
            // Only for control panel objects.
            throw new NotSupportedException();
        }

        /// <summary>
        /// Adds additional info to the registry to allow the shell to discover the oject as shell extension.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="description">The description.</param>
        /// <param name="progId">The prog id.</param>
        protected static void ComRegister(Type type, string description, string progId)
        {
            Contract.Requires(type != null);
            Contract.Requires(!string.IsNullOrEmpty(description));
            Contract.Requires(!string.IsNullOrEmpty(progId));

            RegistryExtensions.AddAsApprovedShellExtension(type, description);

            // Register the object as a property sheet handler.
            var subKeyName = progId + @"\ShellEx\PropertySheetHandlers\" + description;
            using (var key = Registry.ClassesRoot.CreateSubKey(subKeyName))
            {
                if (key == null)
                    throw new ApplicationException("Failed to create sub key: " + subKeyName);

                key.SetValue(string.Empty, type.GUID.ToString("B"));
            }
        }

        /// <summary>
        /// Removed the additional info from the registry that allowed the shell to discover the shell extension.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="description">The description.</param>
        /// <param name="progId">The name.</param>
        protected static void ComUnregister(Type type, string description, string progId)
        {
            Contract.Requires(type != null);
            Contract.Requires(!string.IsNullOrEmpty(description));
            Contract.Requires(!string.IsNullOrEmpty(progId));

            RegistryExtensions.RemoveAsApprovedShellExtension(type);

            using (var key = Registry.ClassesRoot.OpenSubKey(progId + @"\ShellEx\PropertySheetHandlers\", true))
            {
                if (key != null)
                {
                    key.DeleteSubKey(description, false);
                }
            }
        }

        /// <summary>
        /// Called when [add pages].
        /// </summary>
        /// <param name="fileNames">The file names.</param>
        protected abstract void OnAddPages(IList<string> fileNames);
    }
}
