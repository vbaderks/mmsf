// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Flags specifying the information to return.
    /// </summary>
    public enum GetCommandStringOptions
    {
        /// <summary>
        /// Sets pszName to an ANSI string containing the help text for the command (GCS_VERBA).
        /// </summary>
        CanonicalVerbAnsi = 0x0,

        /// <summary>
        /// Sets pszName to a Unicode string containing the language-independent command name for the menu item (GCS_VERBW).
        /// </summary>
        CanonicalVerb = 0x4,

        /// <summary>
        /// Sets pszName to a Unicode string containing the help text for the command (GCS_HELPTEXTW).
        /// </summary>
        HelpText = 0x5,

        /// <summary>
        /// Returns S_OK if the menu item exists, or S_FALSE otherwise.
        /// </summary>
        Validate = 0x6
    }
}
