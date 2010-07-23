// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using Microsoft.Win32;

namespace MiniShellFramework
{
    public static class RootKey
    {
        public static void Register(string fileExtension, string progId)
        {
            using (var key = Registry.ClassesRoot.CreateSubKey(fileExtension))
            {
                key.SetValue(string.Empty, progId);
            }
        }

        public static void Unregister(string fileExtension)
        {
            Registry.ClassesRoot.DeleteSubKey(fileExtension);
        }
    }
}
