// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System.Runtime.InteropServices;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Exposes a method to initialize a handler, such as a property handler, thumbnail handler, or preview handler, with a file path.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("b7d14566-0509-4cce-a71f-0a554233bd9b")]
    public interface IInitializeWithFile
    {
        /// <summary>
        /// Initializes a handler with a file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="storageMode">The storage mode.</param>
        void Initialize([In, MarshalAs(UnmanagedType.LPWStr)] string filePath, StorageModes storageMode);
    }
}
