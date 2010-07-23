// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;

namespace MiniShellFramework
{
    /// <summary>
    /// TODO
    /// </summary>
    public abstract class CustomMenuHandler
    {
        /// <summary>
        /// Initializes the item info.
        /// </summary>
        /// <param name="menuiteminfo">The menuiteminfo.</param>
        public virtual void InitializeItemInfo(ref MENUITEMINFO menuiteminfo)
        {
            menuiteminfo.OwnerDraw = true;
        }

        /// <summary>
        /// Called by OS to require the size of the menu item.
        /// </summary>
        /// <param name="measureItem">The measure item.</param>
        public virtual void Measure(ref MEASUREITEMSTRUCT measureItem)
        {
        }

        /// <summary>
        /// Called by the OS when the item must be draw.
        /// </summary>
        public virtual void Draw(/* DRAWITEMSTRUCT&  */ /*drawitem*/)
        {
        }

        /// <summary>
        /// Override this function to handle accelerator keys
        /// </summary>
        /// <param name="hmenu">The hmenu.</param>
        /// <param name="nChar">The n char.</param>
        /// <returns></returns>
        public virtual bool OnMenuChar(IntPtr hmenu, ushort nChar /*, LRESULT& */ /*lresult*/)
        {
            return false; // not handled.
        }
    }
}
