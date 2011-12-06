// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using MiniShellFramework.ComTypes;

namespace MiniShellFramework
{
    public static class SummaryInformationPropertyStreamIds
    {
        public static Guid Guid
        {
            get
            {
                return new Guid(0xf29f85e0, 0x4ff9, 0x1068, 0xab, 0x91, 0x08, 0x00, 0x2b, 0x27, 0xb3, 0xd9);
            }
        }

        // #define PID_AUTHOR        4  // string
        public const int Author = 4;
    }


    /// <summary>
    /// Provide a base class for property sheet shell extensions.
    /// </summary>
    [ComVisible(true)]                        // Make this .NET class visible to ensure derived class can be COM visible.
    [ClassInterface(ClassInterfaceType.None)] // Only the functions from the COM interfaces should be accessible.
    public abstract class ColumnProviderBase<T> : ShellExtension, IColumnProvider
    {
        private const string ColumnHandlersKeyName = @"Folder\ShellEx\ColumnHandlers";
        private bool initialized;
        private bool hideDesktopColumns;
        private readonly List<ShellColumnInfo> columnInfos = new List<ShellColumnInfo>();
        private readonly List<string> extensions = new List<string>();
        private IList<string> cachedInfo;
        private string cachedFileName;
        private Dictionary<string, List<string>> cachedInfos = new Dictionary<string, List<string>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnProviderBase&lt;T&gt;"/> class.
        /// </summary>
        protected ColumnProviderBase()
        {
            Debug.WriteLine("[{0}] ColumnProviderBase.Constructor ()", Id);
        }

        void IColumnProvider.Initialize(ref ShellColumnInitializeInfo shellColumnInitializeInfo)
        {
            Debug.WriteLine("[{0}] ColumnProviderBase.IColumnProvider.Initialize, shellColumnInitializeInfo.Folder={1}",
                Id, shellColumnInitializeInfo.Folder);

            if (initialized)
                throw new COMException("Already Initialized");

            // Note: Due to a bug in explorer it will not release the interface if the
            //       folder is the 'desktop'. The workaround is to return 0 columns in that case.
            //       This will prevent the resource leak. The drawback is that there are no
            //       columns available if the 'desktop' folder is viewed in 'detailed' mode (which is a rare case).
            hideDesktopColumns = IsDesktopPath(shellColumnInitializeInfo.Folder);

            // Note: InitializeCore needs to be implemented by the derived class.
            InitializeCore(shellColumnInitializeInfo.Folder);

            initialized = true;
        }

        /// <summary>
        /// Requests information about a column.
        /// By calling this repeatedly the shell can detect how many columns there are.
        /// </summary>
        /// <param name="index">The column's zero-based index. It is an arbitrary value that is used to enumerate columns (DWORD dwIndex).</param>
        /// <param name="columnInfo">Information about the columnn (psci).</param>
        int IColumnProvider.GetColumnInfo(int index, ref ShellColumnInfo columnInfo)
        {
            Debug.WriteLine("[{0}] ColumnProviderBase.IColumnProvider.GetColumnInfo, index={1}", Id, index);

            if (!initialized)
                throw new COMException("Initialize was not called");

            if (hideDesktopColumns || index >= columnInfos.Count)
                return HResults.False; // tell the shell there are no more columns.

            columnInfo = columnInfos[index];
            return HResults.Ok;
        }

        int IColumnProvider.GetItemData(ref ShellColumnId columnId, ref ShellColumnData columnData, object data)
        {
            Debug.WriteLine("[{0}] ColumnProviderBase.IColumnProvider.GetItemData, columnId.FormatId={1}, columnId.PropertyId={2}",
                Id, columnId.FormatId, columnId.PropertyId);

            if (!initialized)
                throw new COMException("Initialize was not called");

            if (!IsSupportedItem(ref columnData))
            {
                ////VariantInit(pvarData); // must initialise out args as we return a success code.
                return HResults.False;
            }

            bool flushCache = columnData.Flags.HasFlag(ShellColumnDataOptions.UpdateItem);
            var fileName = Path.GetFileName(columnData.File);
            object info = null;
            if (!flushCache)
            {
                // User of this interface (explorer.exe) are expected to ask 
                // all active item data per file.
                info = FindInLastUsedCache(fileName);
            }

            if (info == null)
            {
                info = GetAndCacheFileInfo(fileName, columnData.File, flushCache);
            }
  
            ////pvarData->bstrVal = (*pCachedInfo)[GetIndex(*pscid)].AllocSysString();
            ////pvarData->vt = VT_BSTR;

            return HResults.Ok;
        }

        /// <summary>
        /// Adds additional info to the registry to allow the shell to discover the oject as shell extension.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="description">The description.</param>
        protected static void ComRegister(Type type, string description)
        {
            Contract.Requires(type != null);
            Contract.Requires(!string.IsNullOrEmpty(description));

            // Only register when supported by Shell.
            // Vista and up don't support column providers anymore (replaces by property system).
            if (IsShell60OrHigher())
                return;

            RegistryExtensions.AddAsApprovedShellExtension(type, description);

            // Register the COM object as a ColumnProvider handler.
            var subKeyName = ColumnHandlersKeyName + @"\" + type.GUID.ToString("B");
            using (var key = Registry.ClassesRoot.CreateSubKey(subKeyName))
            {
                if (key == null)
                    throw new ApplicationException("Failed to create sub key: " + subKeyName);

                key.SetValue(string.Empty, description);
            }
        }

        /// <summary>
        /// Removed the additional info from the registry that allowed the shell to discover the shell extension.
        /// </summary>
        /// <param name="type">The type.</param>
        protected static void ComUnregister(Type type)
        {
            Contract.Requires(type != null);

            // Only try to remove when it was registered.
            if (IsShell60OrHigher())
                return;

            using (var key = Registry.ClassesRoot.OpenSubKey(ColumnHandlersKeyName, true))
            {
                if (key != null)
                {
                    key.DeleteSubKey(type.GUID.ToString("B"), false);
                }
            }

            RegistryExtensions.RemoveAsApprovedShellExtension(type);
        }

        protected abstract void InitializeCore(string folderName);

        // Purpose: provides a default GUID for the columns.
        protected Guid GetStandardFormatIdentifier()
        {
            return Marshal.GenerateGuidForType(typeof(T));
        }

        protected void RegisterColumn(string title, uint defaultWidthInCharacters,
            ListViewAlignment format = ListViewAlignment.Left, ShellColumnState state = ShellColumnState.TypeString)
        {
            Contract.Requires(title != null);
            Contract.Requires(title.Length < ShellColumnInfo.MaxTitleLength);

            RegisterColumn(GetStandardFormatIdentifier(), columnInfos.Count, title, defaultWidthInCharacters, format, state);
        }

        protected void RegisterColumn(Guid formatId, int propertyId, string title, uint defaultWidthInCharacters,
            ListViewAlignment format = ListViewAlignment.Left, ShellColumnState state = ShellColumnState.TypeString)
        {
            Contract.Requires(title != null);
            Contract.Requires(title.Length < ShellColumnInfo.MaxTitleLength);

            var columnId = new ShellColumnId { FormatId = formatId, PropertyId = propertyId };

            // Note: description field is not used by the shell.
            var columnInfo = new ShellColumnInfo();
            columnInfo.ColumnId = columnId;
            columnInfo.Title = title;
            columnInfo.DefaultWidthInCharacters = defaultWidthInCharacters;
            columnInfo.Format = format;
            columnInfo.State = state | ShellColumnState.Extended | ShellColumnState.SecondaryUI;

            // Note: VT_LPSTR/VT_BSTR works ok. Other types seems to have issues with sorting.
            columnInfo.variantType = 0; // TODO = VT_BSTR

            columnInfos.Add(columnInfo);
        }

        protected void RegisterExtension(string extension)
        {
            extensions.Add(extension.ToLowerInvariant());
        }

        protected virtual FileAttributes GetFileAttributeMask()
        {
            return FileAttributes.Directory | FileAttributes.Offline;
        }

        private static bool IsShell60OrHigher()
        {
            return Environment.OSVersion.Version.Major > 6;
        }

        /// <summary>
        /// Checks if the folder is the 'all users' or user desktop folder.
        /// </summary>
        /// <param name="folder"></param>
        private static bool IsDesktopPath(string folder)
        {
            Contract.Requires(folder != null);

            return Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory) == folder ||
                   Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) == folder;
        }

        private bool IsSupportedItem(ref ShellColumnData columnData)
        {
            // Check file mask and file extension.
            return !columnData.FileAttributes.HasFlag(GetFileAttributeMask()) && 
                extensions.Contains(columnData.FileNameExtension.ToLowerInvariant());
        }

        private IList<string> FindInLastUsedCache(string fileName)
        {
            if (cachedInfo == null)
                return null; // last used cache is empty.

            if (cachedFileName != fileName)
                return null; // last used cache was for a different file.

            return cachedInfo;
        }

        private IList<string> GetAndCacheFileInfo(string fileName, string file, bool flushCache)
        {
            IList<string> info;

            if (flushCache)
            {
                info = null;
            }
            else
            {
                info = FindInCache(fileName);
            }

            if (info == null)
            {
                // New file info must be stored in the cache.
                var columnInfos1 = new List<string>();

                // Note: GetAllColumnInfo must be implemented by the derived class.
                ////static_cast<T*>(this)->GetAllColumnInfo(CString(CW2CT(wszFile)), strColumnInfos);
                GetAllColumnInfoCore(fileName, columnInfos1);

                Debug.Assert(columnInfos1.Count == columnInfos.Count,
                    "Mismatch detected between registered columns and returned values.");

                ////pCachedInfo = InsertInCache(strFilename, strColumnInfos);
                ////cachedInfos[fileName] = info;
            }

            return null; //// InsertInLastUsedCache(fileName, cachedInfo);
        }

    ////const std::vector<CString>* InsertInCache(const CStringW& strFilename, const std::vector<CString>& strColumnInfos)
    ////{
    ////    return &(m_mapCacheInfo[strFilename] = strColumnInfos);
    ////}

        private IList<string> FindInCache(string fileName)
        {
            List<string> infos;
            return cachedInfos.TryGetValue(fileName, out infos) ? infos : null;
        }

        protected abstract void GetAllColumnInfoCore(string fileName, IList<string> columnInfos);
    }
}
