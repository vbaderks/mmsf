// <copyright company="Victor Derks">
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using MiniShellFramework;

namespace VvvSample
{
    internal static class VvvRootKey
    {
        static bool registered;

        public const string ProgId = "VVVFile";
        public const string FileExtension = ".vvv";

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
