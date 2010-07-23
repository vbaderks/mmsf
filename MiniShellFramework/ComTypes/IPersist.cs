// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace MiniShellFramework.Comtypes
{
    /// <summary>
    /// Provides the CLSID of an object that can be stored persistently in the system.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("0000010c-0000-0000-C000-000000000046")]
    public interface IPersist
    {
        /// <summary>
        /// Gets the class ID.
        /// </summary>
        /// <param name="classId">The class id.</param>
        void GetClassID(ref Guid classId);
    }
}
