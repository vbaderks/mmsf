// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System.Runtime.InteropServices;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Exposes methods that enable the addition of custom columns in the Windows Explorer Details view.
    /// This interface is used only on Windows 2000 and Windows XP. The new property system is used on Vista and newer.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214FC-0000-0000-c000-000000000046")] // TODO
    ////[ContractClass(typeof(CopyHookContract))]
    public interface IColumnProvider
    {
        /// <summary>
        /// Initializes the specified shell column initialize info.
        /// </summary>
        /// <param name="shellColumnInitializeInfo">
        /// The shell column initialize info with initialization information, including the folder whose contents are to be displayed.
        /// </param>
        void Initialize(ref ShellColumnInitializeInfo shellColumnInitializeInfo);

        /// <summary>
        /// Requests information about a column.
        /// By calling this repeatedly the shell can detect how many columns there are.
        /// </summary>
        /// <param name="index">
        /// The column's zero-based index. It is an arbitrary value that is used to enumerate columns (DWORD dwIndex).
        /// </param>
        /// <param name="columnInfo">
        /// Information about the columnn (psci).
        /// </param>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetColumnInfo(int index, ref ShellColumnInfo columnInfo);

        /// <summary>
        /// Requests column data for a specified file.
        /// </summary>
        /// <param name="columnId">Identifies the column.</param>
        /// <param name="columnData">The column data.</param>
        /// <param name="data">The data.</param>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetItemData(ref ShellColumnId columnId, ref ShellColumnData columnData, object data);
    }
}
