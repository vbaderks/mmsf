// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Runtime.InteropServices;
using System.Text;

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


    /// <summary>
    /// Exposes methods that either create or merge a shortcut menu associated with a Shell object.
    /// Allows client objects to handle messages associated with owner-drawn menu items and extends
    /// IContextMenu2 by accepting a return value from that message handling.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214E4-0000-0000-c000-000000000046")]
    public interface IContextMenu
    {
        /// <summary>
        /// Queries the context menu (from IContextMenu).
        /// </summary>
        /// <param name="menuHandle">A handle to the shortcut menu. The handler should specify this handle when adding menu items (hmenu).</param>
        /// <param name="position">The zero-based position at which to insert the first new menu item (indexMenu).</param>
        /// <param name="firstCommandId">The minimum value that the handler can specify for a menu item identifier (idCmdFirst).</param>
        /// <param name="lastCommandId">The maximum value that the handler can specify for a menu item identifier (idCmdLast).</param>
        /// <param name="flags">Optional flags that specify how the shortcut menu can be changed (uFlags).</param>
        /// <returns>
        /// If successful, returns an HRESULT value that has its severity value set to SEVERITY_SUCCESS and 
        /// its code value set to the offset of the largest command identifier that was assigned, plus one.
        /// </returns>
        [PreserveSig]
        [return:MarshalAs(UnmanagedType.Error)]
        int QueryContextMenu(IntPtr menuHandle, uint position, uint firstCommandId, uint lastCommandId, QueryContextMenuOptions flags);

        /// <summary>
        /// Carries out the command associated with a shortcut menu item (from IContextMenu).
        /// </summary>
        /// <param name="invokeCommandInfo">The invoke command info.</param>
        void InvokeCommand([In] ref InvokeCommandInfo invokeCommandInfo);

        /// <summary>
        /// Gets information about a shortcut menu command, including the help string and the
        /// language-independent, or canonical, name for the command (from IContextMenu).
        /// </summary>
        /// <param name="idCommand">Menu command identifier offset.</param>
        /// <param name="flags">The uflags.</param>
        /// <param name="reserved">The reserved.</param>
        /// <param name="name">The command string.</param>
        /// <param name="cch">The CCH.</param>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetCommandString(IntPtr idCommand, GetCommandStringOptions flags, int reserved, IntPtr name, int cch);
    }
}
