// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Exposes a method that initializes a handler, such as a property handler, thumbnail handler, or preview handler, with a stream.
    /// </summary>
    [ComImport]
    [Guid("b824b49d-22ac-4161-ac8a-9916e8fa3f7f")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ContractClass(typeof(InitializeWithStreamContract))]
    public interface IInitializeWithStream
    {
        /// <summary>
        /// Initializes a handler with a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="storageMode">The storage mode.</param>
        void Initialize([In] IStream stream, StorageModes storageMode);
    }

    [ContractClassFor(typeof(IInitializeWithStream))]
    internal abstract class InitializeWithStreamContract : IInitializeWithStream
    {
        public void Initialize(IStream stream, StorageModes storageMode)
        {
            Contract.Requires(stream != null);
        }
    }
}
