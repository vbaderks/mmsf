// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using MiniShellFramework.ComTypes;

namespace MiniShellFramework
{
    /// <summary>
    /// Provide a base class for property sheet shell extensions.
    /// </summary>
    [ComVisible(true)]                        // Make this .NET class visible to ensure derived class can be COM visible.
    [ClassInterface(ClassInterfaceType.None)] // Only the functions from the COM interfaces should be accessible.
    public abstract class ShellPropertySheetBase : ShellExtensionInit, IShellPropSheetExt
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellPropertySheetBase"/> class.
        /// </summary>
        protected ShellPropertySheetBase()
        {
            Debug.WriteLine("{0}.Constructor (ShellPropertySheetBase)", this);
        }

        int IShellPropSheetExt.AddPages(IntPtr addPageFunction, IntPtr lParam)
        {
            Debug.WriteLine("{0}.IShellPropSheetExt.AddPages (ShellPropertySheetBase), lParam={1}", this, lParam);

            throw new NotImplementedException();
        }

        void IShellPropSheetExt.ReplacePage(uint pageId, IntPtr replaceWithFunction, IntPtr lParam)
        {
            Debug.WriteLine("{0}.IShellPropSheetExt.ReplacePage (ShellPropertySheetBase) - Not Implemented (functionality not used)", this);
            throw new NotSupportedException();
        }
    }
}
