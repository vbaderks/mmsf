// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using MiniShellFramework.ComTypes;

namespace MiniShellFramework
{
    /// <summary>
    /// TODO
    /// </summary>
    public class SmallBitmapCustomMenuHandler : CustomMenuHandler
    {
        private readonly string text;
        private readonly Bitmap bitmap;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmallBitmapCustomMenuHandler"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="bitmap">The res id bitmap.</param>
        public SmallBitmapCustomMenuHandler(string text, Bitmap bitmap)
        {
            Contract.Requires(text != null);
            Contract.Requires(bitmap != null);

            this.text = text;
            this.bitmap = bitmap;
        }

        /// <summary>
        /// Initializes the item info.
        /// </summary>
        /// <param name="menuiteminfo">The menuiteminfo.</param>
        public override void InitializeItemInfo(ref MenuItemInfo menuiteminfo)
        {
            menuiteminfo.Text = text;
            menuiteminfo.SetCheckMarks(IntPtr.Zero, bitmap.GetHbitmap());
        }
    }
}
