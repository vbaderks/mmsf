// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

namespace MiniShellFramework
{
    /// <summary>
    /// TODO
    /// </summary>
    public class SmallBitmapCustomMenuHandler : CustomMenuHandler
    {
        private readonly string text;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmallBitmapCustomMenuHandler"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="resIdBitmap">The res id bitmap.</param>
        public SmallBitmapCustomMenuHandler(string text, int resIdBitmap)
        {
            this.text = text;
        }

        /// <summary>
        /// Initializes the item info.
        /// </summary>
        /// <param name="menuiteminfo">The menuiteminfo.</param>
        public override void InitializeItemInfo(ref MenuItemInfo menuiteminfo)
        {
            menuiteminfo.Text = text;
            ////menuiteminfo.SetCheckMarkBmps(NULL, m_bitmap.GetHandle());
        }
    }
}
