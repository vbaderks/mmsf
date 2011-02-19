// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using MiniShellFramework;

namespace VvvSample
{
    internal static class VvvRootKey
    {
        // The recommended versioned ProgID (MSDN docs) has the format: ProductName.extension.versionMajor.versionMinor
        public const string ProgId = "MVVVFile.mvvv.1.0";

        public const string FileExtension = ".mvvv";
        private static bool registered;

        public static void Register()
        {
            if (registered)
                return;

            RootKey.Register(FileExtension, ProgId);
            registered = true;

            //// SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, NULL, NULL);
        }

        public static void Unregister()
        {
            if (!registered)
                return;

            // In this sample the extension owns the file extension and ProdId, which makes it save to remove
            // it during unregistration. This is often not the case for file extension for application by other vendors.
            // In those cases the file extension and ProdID should not be removed.
            RootKey.Unregister(FileExtension, ProgId);
            registered = false;

            //// SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, NULL, NULL);
        }
    }
}
