// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Text;
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
    public interface IContextMenu3
    {
        /// <summary>
        /// Queries the context menu (from IContextMenu).
        /// </summary>
        /// <param name="handleMenu">A handle to the shortcut menu. The handler should specify this handle when adding menu items.</param>
        /// <param name="indexMenu">The zero-based position at which to insert the first new menu item.</param>
        /// <param name="idCommandFirst">The minimum value that the handler can specify for a menu item identifier.</param>
        /// <param name="idCommandLast">The maximum value that the handler can specify for a menu item identifier.</param>
        /// <param name="flags">Optional flags that specify how the shortcut menu can be changed.</param>
        /// <returns></returns>
        [PreserveSig]
        int QueryContextMenu(IntPtr handleMenu, uint indexMenu, uint idCommandFirst, uint idCommandLast, QueryContextMenuOptions flags);

        /// <summary>
        /// Carries out the command associated with a shortcut menu item (from IContextMenu).
        /// </summary>
        /// <param name="invokeCommandInfo">The invoke command info.</param>
        void InvokeCommand([In] ref InvokeCommandInfo invokeCommandInfo);

        /// <summary>
        /// Gets information about a shortcut menu command, including the help string and the
        /// language-independent, or canonical, name for the command  (from IContextMenu).
        /// </summary>
        /// <param name="idCommand">Menu command identifier offset.</param>
        /// <param name="flags">The uflags.</param>
        /// <param name="reserved">The reserved.</param>
        /// <param name="commandString">The command string.</param>
        /// <param name="cch">The CCH.</param>
        [PreserveSig]
        int GetCommandString(IntPtr idCommand, uint flags, int reserved, StringBuilder commandString, int cch);

        /// <summary>
        /// Enables client objects of the IContextMenu interface to handle messages
        /// associated with owner-drawn menu items (from IContextMenu2).
        /// </summary>
        /// <param name="uMsg">The u MSG.</param>
        /// <param name="wParam">The w param.</param>
        /// <param name="lParam">The l param.</param>
        /// <returns></returns>
        void HandleMenuMsg(uint uMsg, uint wParam, uint lParam);

        /// <summary>
        /// Allows client objects of the IContextMenu3 interface to handle messages associated with owner-drawn menu items.
        /// </summary>
        /// <param name="uMsg">The u MSG.</param>
        /// <param name="wParam">The w param.</param>
        /// <param name="lParam">The l param.</param>
        /// <param name="plResult">The pl result.</param>
        void HandleMenuMsg2(uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr plResult);
    }
}
