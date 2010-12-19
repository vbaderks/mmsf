// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace MiniShellFramework
{
    /// <summary>
    /// This class contains methods to work with FORMATETC.
    /// </summary>
    public static class FormatEtcExtensions
    {
        /// <summary>
        /// Creates the FORMATETC.
        /// </summary>
        /// <param name="clipFormat">The clip format.</param>
        /// <returns>A FORMATETC that is initialized with the clip format.</returns>
        public static FORMATETC CreateFORMATETC(ClipFormat clipFormat)
        {
            return new FORMATETC
                             {
                                     cfFormat = (short)clipFormat,
                                     tymed = TYMED.TYMED_HGLOBAL,
                                     dwAspect = DVASPECT.DVASPECT_CONTENT,
                                     ptd = IntPtr.Zero,
                                     lindex = -1
                             };
        }

        /// <summary>
        /// Creates the STGMEDIUM.
        /// </summary>
        /// <returns>A STGMEDIUM instance.</returns>
        public static STGMEDIUM CreateSTGMEDIUM()
        {
            var medium = new STGMEDIUM();

            return medium;
        }

    }
}
