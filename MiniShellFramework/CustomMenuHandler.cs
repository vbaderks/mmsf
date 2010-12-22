// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using MiniShellFramework.ComTypes;

namespace MiniShellFramework
{
    /// <summary>
    /// TODO
    /// </summary>
    public abstract class CustomMenuHandler
    {
        /// <summary>
        /// TODO
        /// </summary>
        public const ushort MenuCharResultIgnore = 0;

        /// <summary>
        /// TODO
        /// </summary>
        public const ushort MenuCharResultClose = 1;

        /// <summary>
        /// TODO
        /// </summary>
        public const ushort MenuCharResultExecute = 2;

        /// <summary>
        /// TODO
        /// </summary>
        public const ushort MenuCharResultSelect = 3;

        /// <summary>
        /// Initializes the item info.
        /// </summary>
        /// <param name="menuiteminfo">The menuiteminfo.</param>
        public virtual void InitializeItemInfo(ref MenuItemInfo menuiteminfo)
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
        public virtual void Draw(ref DrawItem drawItem)
        {
        }

        /// <summary>
        /// Override this function to handle accelerator keys
        /// </summary>
        /// <param name="hmenu">The hmenu.</param>
        /// <param name="nChar">The n char.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public virtual bool OnMenuChar(IntPtr hmenu, ushort nChar, out ushort result)
        {
            result = default(ushort);
            return false; // not handled.
        }
    }
}
