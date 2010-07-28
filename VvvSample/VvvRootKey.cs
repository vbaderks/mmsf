// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using MiniShellFramework;

namespace VvvSample
{
    internal static class VvvRootKey
    {
        public const string ProgId = "MVVVFile";
        public const string FileExtension = ".mvvv";
        private static bool registered;

        public static void Register()
        {
            if (registered)
                return;

            RootKey.Register(FileExtension, ProgId);
            registered = true;
        }

        public static void Unregister()
        {
            if (!registered)
                return;

            RootKey.Unregister(FileExtension);
            registered = false;
        }
    }
}
