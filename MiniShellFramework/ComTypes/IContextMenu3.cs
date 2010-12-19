// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Exposes methods that either create or merge a shortcut menu associated with a Shell object.
    /// Allows client objects to handle messages associated with owner-drawn menu items and extends
    /// IContextMenu2 by accepting a return value from that message handling.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [GuidAttribute("bcfce0a0-ec17-11d0-8d10-00a0c90f2719")]
    public interface IContextMenu3 : IContextMenu2
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
        /// its code value set to the offset of the largest command identifier that was assigned.
        /// </returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        new int QueryContextMenu(IntPtr menuHandle, uint position, uint firstCommandId, uint lastCommandId, QueryContextMenuOptions flags);

        /// <summary>
        /// Carries out the command associated with a shortcut menu item (from IContextMenu).
        /// </summary>
        /// <param name="invokeCommandInfo">The invoke command info.</param>
        new void InvokeCommand([In] ref InvokeCommandInfo invokeCommandInfo);

        /// <summary>
        /// Gets information about a shortcut menu command, including the help string and the
        /// language-independent, or canonical, name for the command  (from IContextMenu).
        /// </summary>
        /// <param name="idCommand">Menu command identifier offset.</param>
        /// <param name="flags">The uflags.</param>
        /// <param name="reserved">The reserved.</param>
        /// <param name="name">The command string.</param>
        /// <param name="cch">The CCH.</param>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        new int GetCommandString(IntPtr idCommand, GetCommandStringOptions flags, int reserved, IntPtr name, int cch);

        /// <summary>
        /// Enables client objects of the IContextMenu interface to handle messages
        /// associated with owner-drawn menu items (from IContextMenu2).
        /// </summary>
        /// <param name="message">The message to be processed.</param>
        /// <param name="wParam">Additional message information. The value of the wParam parameter depends on the value of the message parameter.</param>
        /// <param name="lParam">Additional message information. The value of the lParam parameter depends on the value of the message parameter.</param>
        new void HandleMenuMsg(uint message, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Allows client objects of the IContextMenu3 interface to handle messages associated with owner-drawn menu items.
        /// </summary>
        /// <param name="message">The message to be processed.</param>
        /// <param name="wParam">Additional message information. The value of the wParam parameter depends on the value of the message parameter.</param>
        /// <param name="lParam">Additional message information. The value of the lParam parameter depends on the value of the message parameter.</param>
        /// <param name="result">A pointer to the result.</param>
        void HandleMenuMsg2(uint message, IntPtr wParam, IntPtr lParam, IntPtr result);
    }
}
