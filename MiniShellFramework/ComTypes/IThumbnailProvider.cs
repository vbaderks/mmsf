// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Exposes a method for getting a thumbnail image.
    /// </summary>
    [ComImport]
    [Guid("e357fccd-a995-4576-b01f-234630154e96")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ContractClass(typeof(ThumbnailProviderContract))]
    public interface IThumbnailProvider
    {
        /// <summary>
        /// Gets a thumbnail image and alpha type.
        /// </summary>
        /// <param name="squareLength">
        /// The maximum thumbnail size, in pixels. The Shell draws the returned bitmap at this size or smaller.
        /// The returned bitmap should fit into a square of width and height cx, though it does not need to be a square image.
        /// The Shell scales the bitmap to render at lower sizes. For example, if the image has a 6:4 aspect ratio, then the
        /// returned bitmap should also have a 6:4 aspect ratio..</param>
        /// <param name="bitmapHandle">
        /// When this method returns, contains a pointer to the thumbnail image handle. The image must be a DIB section
        /// and 32 bits per pixel. The Shell scales down the bitmap if its width or height is larger than the size 
        /// specified by cx. The Shell always respects the aspect ratio and never scales a bitmap larger than its original size.</param>
        /// <param name="alphaType">When this method returns, contains a one of the following values from the ThumbnailAlphaType enumeration:</param>
        void GetThumbnail(uint squareLength, [Out] out IntPtr bitmapHandle, [Out] out ThumbnailAlphaType alphaType);
    }

    [ContractClassFor(typeof(IThumbnailProvider))]
    internal abstract class ThumbnailProviderContract : IThumbnailProvider
    {
        public void GetThumbnail(uint squareLength, [Out] out IntPtr bitmapHandle, [Out] out ThumbnailAlphaType alphaType)
        {
            bitmapHandle = default(IntPtr);
            alphaType = default(ThumbnailAlphaType);
        }
    }
}
