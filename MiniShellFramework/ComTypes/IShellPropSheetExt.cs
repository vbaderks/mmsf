// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Callback, passed to 'AddPages'.
    /// </summary>
    public delegate bool AddPropertySheetPage(IntPtr propertySheetPageHandle, IntPtr lParam);

    /// <summary>
    /// Exposes methods that allow a property sheet handler to add or replace pages in the property sheet displayed for a file object.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214E9-0000-0000-C000-000000000046")]
    public interface IShellPropSheetExt
    {
        /// <summary>
        /// Adds one or more pages to a property sheet that the Shell displays for a file object.
        /// The Shell calls this method for each property sheet handler registered to the file type.
        /// </summary>
        /// <param name="addPageFunction">
        /// A pointer to a function that the property sheet handler calls to add a page to the property sheet.
        /// The function takes a property sheet handle returned by the CreatePropertySheetPage function and
        /// the lParam parameter passed to this method.</param>
        /// <param name="lParam">Handler-specific data to pass to the function pointed to by pfnAddPage.</param>
        /// <returns>If successful, returns a one-based index to specify the page that should be initially displayed.</returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        ////int AddPages(AddPropertySheetPage addPageFunction, IntPtr lParam);
        int AddPages(IntPtr addPageFunction, IntPtr lParam);

        // TODO: process
        ////[PreserveSig()]
        ////int AddPages([MarshalAs(UnmanagedType.FunctionPtr)] LPFNSVADDPROPSHEETPAGE pfnAddPage, IntPtr lParam);

        ////[DllImport("comctl32.dll")]
        ////private static extern IntPtr DestroyPropertySheetPage(IntPtr hProp);
        //// Marshal.GetDelegateForFunctionPointer(pfnAddPag

        /// <summary>
        /// Replaces a page in a property sheet for a Control Panel object.
        /// </summary>
        /// <param name="pageId">Windows XP and earlier: a type EXPPS identifier of the page to replace.</param>
        /// <param name="replaceWithFunction">
        /// A pointer to a function that the property sheet handler calls to replace a page to the property sheet.
        /// The function takes a property sheet handle returned by the CreatePropertySheetPage function and
        /// the lParam parameter passed to the ReplacePage method.</param>
        /// <param name="lParam">The parameter to pass to the function specified by the pfnReplacePage parameter.</param>
        void ReplacePage(uint pageId, IntPtr replaceWithFunction, IntPtr lParam);
    }
}
