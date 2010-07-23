// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Definitions of file operations ().
    /// </summary>
    public enum FileOperation
    {
        /// <summary>
        /// Moves the files \ folders (FO_MOVE).
        /// </summary>
        Move = 1,

        /// <summary>
        /// Copies the files \ folders (FO_COPY).
        /// </summary>
        Copy = 2,

        /// <summary>
        /// Deletes the files \ folders (FO_DELETE).
        /// </summary>
        Delete = 3,

        /// <summary>
        /// Renames the files \ folders (FO_RENAME).
        /// </summary>
        Rename = 4
    }
}