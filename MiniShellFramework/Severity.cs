// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

namespace MiniShellFramework
{
    /// <summary>
    /// Specifies if a HResult value indicates an information, warning or error.
    /// </summary>
    public enum Severity
    {
        /// <summary>
        /// The HResult specifies a success \ informantion.
        /// </summary>
        Success = 0,

        /// <summary>
        /// The HResult specifies an error.
        /// </summary>
        Error = 1
    }
}
