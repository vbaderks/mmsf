// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Defines the alpha options for a thumbnail image (WTS_ALPHATYPE).
    /// </summary>
    public enum ThumbnailAlphaType
    {
        /// <summary>
        /// The bitmap is an unknown format. The Shell tries nonetheless to detect whether the image has an alpha channel.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The bitmap is an RGB image without alpha. The alpha channel is invalid and the Shell ignores it.
        /// </summary>
        Rgb = 1,

        /// <summary>
        /// The bitmap is an ARGB image with a valid alpha channel.
        /// </summary>
        Argb = 2
    }
}
