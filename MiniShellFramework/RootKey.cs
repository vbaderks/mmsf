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
        /// <remarks>
        /// Removeing the file extension and ProgID registry entries should only be done if the 'entries' are owned.
        /// Often these entries are owned by application from other vendors and then these registry keys should not be
        /// removed to prevent breaking the registration from that other application(s).
        /// </remarks>
        /// <param name="fileExtension">The file extension.</param>
        /// <param name="progId">Optional the ProgID. If not null ProgID will also be removed from registry.</param>
        public static void Unregister(string fileExtension, string progId = null)
        {
            Contract.Requires(fileExtension != null);

            Registry.ClassesRoot.DeleteSubKey(fileExtension, false);
        }

        /// <summary>
        /// Tries the get ProgID for file exension.
        /// </summary>
        /// <param name="fileExtension">The file extension.</param>
        /// <param name="progId">The prog id.</param>
        /// <returns>true if the ProgID for that file extension could be found.</returns>
        public static bool TryGetProgIdForFileExension(string fileExtension, out string progId)
        {
            Contract.Requires(fileExtension != null);

            progId = null;
            using (var key = Registry.ClassesRoot.OpenSubKey(fileExtension))
            {
                if (key != null)
                {
                    progId = key.GetValue(string.Empty) as string;
                }
            }

            return progId != null;
        }
    }
}
