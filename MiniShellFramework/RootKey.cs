// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics.Contracts;
using Microsoft.Win32;

namespace MiniShellFramework
{
    /// <summary>
    /// Provides support to register / unregister the 'root' key of a shell extension.
    /// </summary>
    public static class RootKey
    {
        /// <summary>
        /// Registers the specified file extension.
        /// </summary>
        /// <param name="fileExtension">The file extension.</param>
        /// <param name="progId">The prog id.</param>
        public static void Register(string fileExtension, string progId)
        {
            Contract.Requires(fileExtension != null);
            Contract.Requires(progId != null);

            using (var key = Registry.ClassesRoot.CreateSubKey(fileExtension))
            {
                if (key == null)
                    throw new ApplicationException("Failed to create registry sub key: " + fileExtension);

                key.SetValue(string.Empty, progId);
            }
        }

        /// <summary>
        /// Unregisters the specified file extension.
        /// </summary>
        /// <param name="fileExtension">The file extension.</param>
        public static void Unregister(string fileExtension)
        {
            Contract.Requires(fileExtension != null);
            Registry.ClassesRoot.DeleteSubKey(fileExtension, false);
        }
    }
}
