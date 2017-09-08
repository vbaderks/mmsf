// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace MiniShellFramework
{
    /// <summary>
    /// Provide a base class for shell extensions.
    /// </summary>
    [ComVisible(true)] // Make this .NET class COM visible to ensure derived classes can be COM visible.
    public abstract class ShellExtension : ICustomQueryInterface
    {
        private static int nextId;

        CustomQueryInterfaceResult ICustomQueryInterface.GetInterface(ref Guid iid, out IntPtr ppv)
        {
            if (!Guids.TryGetName(iid, out var interfaceName))
            {
                interfaceName = "Unknown COM Interface GUID";
            }
            Debug.WriteLine("[{0}] ShellExtension.ICustomQueryInterface.GetInterface (iid = {1} ({2}))", Id, iid, interfaceName);

            ppv = IntPtr.Zero;

            // Force COM to use its own standard Marshaller and not the .NET runtime managed (free threaded) marshaler.
            return iid == Guids.MarshalInterfaceId ? CustomQueryInterfaceResult.Failed : CustomQueryInterfaceResult.NotHandled;
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        protected int Id { get; } = Interlocked.Increment(ref nextId);

        protected virtual CustomQueryInterfaceResult GetInterfaceCore(ref Guid iid, ref IntPtr ppv)
        {
            return CustomQueryInterfaceResult.NotHandled;
        }
    }
}
