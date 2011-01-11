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
        /// Register the passed type as an approved shell extension. Explorer will only execute approved extensions.
        /// </summary>
        /// <remarks>
        /// To support registry harvest tools such as wix heat.exe the 'Shell Extensions' key will be created when
        /// it is not present. On x64 systems 'Shell Extensions' is not present by default in the wow64node tree.
        /// The side effect is that registration will succeeded, but the shell extension will not work when 
        /// registering it on a x64 system with a 32 bit version of regasm.
        /// On a x64 system the x64 version of regasm should be used to register the shell extension in the
        /// correct registry hive to ensure explorer.exe (which is 64 bit on a x64 platform will pick it up).
        /// Administrator rights are needed to update this part of the registry.
        /// </remarks>
        /// <param name="type">The type that identifies the COM shell extension.</param>
        /// <param name="description">The description text that will be stored in the registry.
        /// Some 3rd party tools will display this string when listing which shell extensions are registered.</param>
        public static void AddAsApprovedShellExtension(Type type, string description)
        {
            Contract.Requires(type != null);
            Contract.Requires(description != null);

            using (var key = Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved"))
            {
                if (key == null)
                    throw new ApplicationException(
                            @"Failed to open registry key Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved");

                key.SetValue(type.GUID.ToString("B"), description);
            }
        }

        /// <summary>
        /// Removes the type as an approved shell extension.
        /// </summary>
        /// <remarks>
        /// Administrator rights are needed to update this part of the registry.
        /// </remarks>
        /// <param name="type">The type that identifies the COM shell extension.</param>
        public static void RemoveAsApprovedShellExtension(Type type)
        {
            Contract.Requires(type != null);

            // Unregister the COM object as an approved shell extension.
            // It is possible to unregister twice, need to be prepared to handle that case.
            using (var key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved", true))
            {
                if (key != null)
                {
                    key.DeleteValue(type.GUID.ToString("B"), false);
                }
            }
        }
    }
}
