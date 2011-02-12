// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using MiniShellFramework.ComTypes;

namespace MiniShellFramework
{
    /// <summary>
    /// Provide a base class for property sheet shell extensions.
    /// </summary>
    [ComVisible(true)]                        // Make this .NET class visible to ensure derived class can be COM visible.
    [ClassInterface(ClassInterfaceType.None)] // Only the functions from the COM interfaces should be accessible.
    public abstract class ColumnProviderBase : ShellExtension, IColumnProvider
    {
        const string ColumnHandlersKeyName = @"Folder\ShellEx\ColumnHandlers";
        private bool initialized;
        private bool hideDesktopColumns;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnProviderBase"/> class.
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
        void IColumnProvider.GetColumnInfo(int index, ref ShellColumnInfo columnInfo)
        {
            Debug.WriteLine("[{0}] ColumnProviderBase.IColumnProvider.GetColumnInfo, index={1}", Id, index);

            if (!initialized)
                throw new COMException("Initialize was not called");

            //if (m_desktopbugworkaround() || dwIndex >= m_columninfos.size())
            //    return S_FALSE; // tell the shell there are no more columns.

            //*psci = m_columninfos[dwIndex];
            //return S_OK;
        }

        void IColumnProvider.GetItemData(ref ShellColumnId columnId, ref ShellColumnData columnData, object data)
        {
            Debug.WriteLine("[{0}] ColumnProviderBase.IColumnProvider.GetItemData, columnId.FormatId={1}, columnId.PropertyId={2}",
                Id, columnId.FormatId, columnId.PropertyId);

            if (!initialized)
                throw new COMException("Initialize was not called");

            //if (!IsSupportedItem(*pscd))
            //{
            //    VariantInit(pvarData); // must initialise out args as we return a success code.
            //    return S_FALSE;
            //}

            //PCWSTR wszFilename = PathFindFileNameW(pscd->wszFile);

            //bool bFlushCache = IsBitSet(pscd->dwFlags, SHCDF_UPDATEITEM);
            //const std::vector<CString>* pCachedInfo;

            //if (bFlushCache)
            //{
            //    pCachedInfo = NULL;
            //}
            //else
            //{
            //    // User of this interface (explorer.exe) are expected to ask 
            //    // all active item data per file.
            //    pCachedInfo = FindInLastUsedCache(wszFilename);
            //}

            //if (pCachedInfo == NULL)
            //{
            //    pCachedInfo = GetAndCacheFileInfo(wszFilename, pscd->wszFile, bFlushCache);
            //}

            //pvarData->bstrVal = (*pCachedInfo)[GetIndex(*pscid)].AllocSysString();
            //pvarData->vt = VT_BSTR;

            //return S_OK;
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

        protected void RegisterColumn(Guid formatId, int propertyId, string title, uint defaultWidthInCharacters,
            ListViewAlignment format = ListViewAlignment.Left, ShellColumnState state = ShellColumnState.TypeString)
        {
            Contract.Requires(title != null);
            Contract.Requires(title.Length < ShellColumnInfo.MaxTitleLength);

            var columnId = new ShellColumnId();
            columnId.FormatId = formatId;
            columnId.PropertyId = propertyId;

            // Note: description field is not used by the shell.
            var columnInfo = new ShellColumnInfo();
            columnInfo.ColumnId = columnId;
            columnInfo.Title = title;
            columnInfo.DefaultWidthInCharacters = defaultWidthInCharacters;
            columnInfo.Format = format;
            columnInfo.State = state | ShellColumnState.Extended | ShellColumnState.SecondaryUI;

            // Note: VT_LPSTR/VT_BSTR works ok. Other types seems to have issues with sorting.
            columnInfo.variantType = 0; // TODO = VT_BSTR

            //    m_columninfos.push_back(columninfo);
            //    m_columnidtoindex[columnid] = static_cast<unsigned int>(m_columninfos.size() - 1);
        }

        protected abstract void InitializeCore(string folderName);

        private static bool IsShell60OrHigher()
        {
            return Environment.OSVersion.Version.Major > 6;
        }

        /// <summary>
        /// Checks if the folder is the 'all users' or user desktop folder.
        /// </summary>
        /// <param name="folder"></param>
        static bool IsDesktopPath(string folder)
        {
            return false;
            //return GetFolderPath(CSIDL_COMMON_DESKTOPDIRECTORY) == wszFolder ||
            //    GetFolderPath(CSIDL_DESKTOPDIRECTORY) == wszFolder;
        }
    }
}
