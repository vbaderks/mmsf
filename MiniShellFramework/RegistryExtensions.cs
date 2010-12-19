// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics.Contracts;
using Microsoft.Win32;

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
}
