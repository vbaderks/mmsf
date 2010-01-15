// <copyright company="Victor Derks">
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;

namespace MiniShellFramework
{
    public abstract class CustomMenuHandler
    {
        public virtual void InitializeItemInfo(ref MENUITEMINFO menuiteminfo)
        {
            menuiteminfo.OwnerDraw = true;
        }

        // Purpose: called by OS to require the size of the menu item.
        public virtual void Measure(ref MEASUREITEMSTRUCT measureItem)
        {
        }

        // Purpose: called by the OS when the item must be draw.
        public virtual void Draw(/* DRAWITEMSTRUCT&  */ /*drawitem*/)
        {
        }

        // Purpose: override this function to handle accelerator keys
        public virtual bool OnMenuChar(IntPtr hmenu, ushort nChar /*, LRESULT& */ /*lresult*/)
        {
            return false; // not handled.
        }
    }
}
