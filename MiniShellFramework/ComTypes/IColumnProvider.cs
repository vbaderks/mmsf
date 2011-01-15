// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Passes initialization information to IColumnProvider::Initialize.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SHCOLUMNINIT
    {
        /// <summary>
        /// Initialization flags. Reserved. Set to 0 (dwFlags).
        /// </summary>
        public int Flags;

        /// <summary>
        /// Reserved for future use. Set to NULL (dwReserved).
        /// </summary>
        public int Reserved;

        /// <summary>
        /// Fully qualified folder path. Empty if multiple folders are specified (wszFolder[MAX_PATH]).
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string Folder;
    }

    /// <summary>
    /// Specifies the format and property identifier of a column that will be displayed by the Windows Explorer Details view.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SHCOLUMNID
    {
        /// <summary>
        /// A property set format identifier or FMTID, a GUID (fmtid).
        /// </summary>
        public Guid FormatId;

        /// <summary>
        /// The column's property identifier (pid).
        /// </summary>
        public int PropertyId;
    }

    /// <summary>
    /// Defines options for alignment of the column header and the subitem text in the column.
    /// </summary>
    public enum ListViewColumnFormat
    {
        /// <summary>
        /// Text is left-aligned (LVCFMT_LEFT).
        /// </summary>
        Left

        // TODO: add more + correct values.
    }

    /// <summary>
    /// Flags indicating the default column state (SHCOLSTATE).
    /// </summary>
    public enum ShellColumnState
    {
        /// <summary>
        /// Date type is a string.
        /// </summary>
        TypeString,

        /// <summary>
        /// Date type is a integer.
        /// </summary>
        TypeInteger

        // TODO: add more + correct values.
    }

    /// <summary>
    /// Contains information about the properties of a column. It is used by IColumnProvider::GetColumnInfo (SHCOLUMNINFO).
    /// </summary>
    [StructLayout(LayoutKind.Sequential)] // + TODO: packing of 1
    public struct SHCOLUMNINFO
    {
        /// <summary>
        /// A column identifier that uniquely identifies the column (scid).
        /// </summary>
        public SHCOLUMNID ColumnId;

        //VARTYPE     vt;                             // OUT the native type of the data returned

        /// <summary>
        /// Format of the column. This member is normally set to Left (fmt).
        /// </summary>
        public ListViewColumnFormat Format;

        /// <summary>
        /// The default width of the column, in characters (cChars).
        /// </summary>
        public uint WidthInCharacters;

        /// <summary>
        /// Flags indicating the default column state (csFlags).
        /// </summary>
        public ShellColumnState State;

        /// <summary>
        /// A null-terminated Unicode string with the column's title (wszTitle[MAX_COLUMN_NAME_LEN]).
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string Title;

        /// <summary>
        /// A null-terminated Unicode string with the column's description (wszDescription[MAX_COLUMN_DESC_LEN]).
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string Description;
    }

    /// <summary>
    /// Contains information that identifies a particular file. It is used by IColumnProvider::GetItemData when requesting data for a particular file.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)] // + TODO pack 8
    public struct SHCOLUMNDATA
    {
        /// <summary>
        /// combination of SHCDF_ flags (dwFlags).
        /// </summary>
        public int Flags;

        /// <summary>
        /// File attributes (dwFileAttributes).
        /// </summary>
        public int FileAttributes;

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        private readonly int Reserved;

        /// <summary>
        /// Address of file name extension.
        /// </summary>
        public IntPtr FileNameExtension;

        /// <summary>
        /// Absolute path of file (wszFile[MAX_PATH]).
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string File;
    }



    /// <summary>
    /// Exposes methods that enable the addition of custom columns in the Windows Explorer Details view.
    /// This interface is used only on Windows 2000 and Windows XP. The new property system is used on Vista and newer.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214FC-0000-0000-c000-000000000046")] // TODO
    //[ContractClass(typeof(CopyHookContract))]
    public interface IColumnProvider
    {
        /// <summary>
        /// Initializes the specified shell column initialize info.
        /// </summary>
        /// <param name="shellColumnInitializeInfo">
        /// The shell column initialize info with initialization information, including the folder whose contents are to be displayed.
        ///</param>
        void Initialize(ref SHCOLUMNINIT shellColumnInitializeInfo);

        /// <summary>
        /// Requests information about a column.
        /// By calling this repeatedly the shell can detect how many columns there are.
        /// </summary>
        /// <param name="index">
        /// The column's zero-based index. It is an arbitrary value that is used to enumerate columns (DWORD dwIndex).
        ///</param>
        /// <param name="columnInfo">
        /// Information about the columnn (psci).
        /// </param>
        void GetColumnInfo(int index, ref SHCOLUMNINFO columnInfo);

        /// <summary>
        /// Requests column data for a specified file.
        /// </summary>
        /// <param name="columnId">Identifies the column.</param>
        /// <param name="columnData">The column data.</param>
        void GetItemData(ref SHCOLUMNID columnId, ref SHCOLUMNDATA columnData /*, VARIANT* pvarData */);
    }
}
