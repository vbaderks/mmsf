// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Defines constants flags that indicate conditions for creating and
    /// deleting the object and access modes for the object (STGM constants).
    /// </summary>
    [Flags]
    public enum StorageModes
    {
        /// <summary>
        /// Indicates that the object is read-only, meaning that modifications cannot be made (STGM_READ).
        /// </summary>
        Read = 0x0,

        /// <summary>
        /// Enables access and modification of object data (STGM_READWRITE).
        /// </summary>
        ReadWrite = 0x2
    }
}
