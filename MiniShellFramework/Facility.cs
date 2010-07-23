// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

namespace MiniShellFramework
{
    /// <summary>
    /// Specifies the facility (sub-system) that the HResult value belongs too.
    /// </summary>
    public enum Facility
    {
        /// <summary>
        /// No facility is defined or the globlal facility (FACILITY_NULL).
        /// </summary>
        None = 0,

        /// <summary>
        /// The interface facility. The HResult value is unique in the scope of the COM interface (FACILITY_ITF).
        /// </summary>
        Interface = 4
    }
}
