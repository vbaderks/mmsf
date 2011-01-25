// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Specifies the format and property identifier of a column that will be displayed by the Windows Explorer Details view (SHCOLUMNID).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ShellColumnId
    {
        /// <summary>
        /// A property set format identifier or FMTID, a GUID (fmtid).
        /// </summary>
        public Guid FormatId;

        /// <summary>
        /// The column's property identifier (pid).
        /// </summary>
        public int PropertyId;
    }
}
