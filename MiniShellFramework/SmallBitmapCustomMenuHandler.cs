// <copyright company="Victor Derks">
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

namespace MiniShellFramework
{
    public class SmallBitmapCustomMenuHandler : CustomMenuHandler
    {
        private readonly string text;

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
