using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniShellFramework
{
    public class SmallBitmapCustomMenuHandler : CustomMenuHandler
    {
        private string text;

        public SmallBitmapCustomMenuHandler(string text, int resIdBitmap)
        {
            this.text = text;
        }

        public override void InitializeItemInfo(ref MENUITEMINFO menuiteminfo)
        {
            menuiteminfo.Text = text;
            ////menuiteminfo.SetCheckMarkBmps(NULL, m_bitmap.GetHandle());
        }
    }
}
