// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System.Runtime.InteropServices;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Exposes methods that the Shell uses to retrieve flags and info tip information for an item that resides 
    /// in an IShellFolder implementation. Info tips are usually displayed inside a ToolTip control.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00021500-0000-0000-C000-000000000046")]
    public interface IQueryInfo
    {
        /// <summary>
        /// Gets the info tip text for an item.
        /// </summary>
        /// <param name="options">Flags directing the handling of the item from which you're retrieving the info tip text. This value is commonly zero (QITIPF_DEFAULT).</param>
        /// <param name="text">The info tip text.</param>
        void GetInfoTip([In] InfoTipOptions options, [Out, MarshalAs(UnmanagedType.LPWStr)] out string text);

        /// <summary>
        /// Gets the information flags for an item.
        /// </summary>
        /// <param name="options">The options.</param>
        void GetInfoFlags([Out] out InfoTipOptions options);
    }
}
