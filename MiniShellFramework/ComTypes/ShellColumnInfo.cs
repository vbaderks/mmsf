// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System.Runtime.InteropServices;

namespace MiniShellFramework.ComTypes
{
    /// <summary>
    /// Contains information about the properties of a column. It is used by IColumnProvider::GetColumnInfo (SHCOLUMNINFO).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ShellColumnInfo
    {
        public const int MaxTitleLength = 80;
        public const int MaxDescriptionLength = 128;

        /// <summary>
        /// A column identifier that uniquely identifies the column (scid).
        /// </summary>
        public ShellColumnId ColumnId;

        /// <summary>
        /// The native type of the data returned (vt).
        /// </summary>
        public ushort variantType;

        /// <summary>
        /// Format of the column. This member is normally set to Left (fmt).
        /// </summary>
        public ListViewAlignment Format;

        /// <summary>
        /// The default width of the column, in characters (cChars).
        /// </summary>
        public uint DefaultWidthInCharacters;

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
}
