// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

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
    public abstract class ColumnProviderBase : ShellExtension, IColumnProvider
    {
        private bool initialized;

        void IColumnProvider.Initialize(ref SHCOLUMNINIT shellColumnInitializeInfo)
        {
            Debug.WriteLine("[{0}] ColumnProviderBase.IColumnProvider.Initialize, shellColumnInitializeInfo.Folder={1}",
                Id, shellColumnInitializeInfo.Folder);

            if (initialized)
                throw new COMException("Already Initialized");

            //// Clear internal caching variables.
            //m_pCachedInfo = NULL;
            //m_strCachedFilename.Empty();
            //m_mapCacheInfo.clear();

            //m_desktopbugworkaround.Initialize(psci->wszFolder);

            //m_columninfos.clear();
            //m_columnidtoindex.clear();

            // Note: OnInitialize needs to be implemented by the derived class.
            InitializeCore(shellColumnInitializeInfo.Folder);

            initialized = true;
        }

        /// <summary>
        /// Requests information about a column.
        /// By calling this repeatedly the shell can detect how many columns there are.
        /// </summary>
        /// <param name="index">The column's zero-based index. It is an arbitrary value that is used to enumerate columns (DWORD dwIndex).</param>
        /// <param name="columnInfo">Information about the columnn (psci).</param>
        void IColumnProvider.GetColumnInfo(int index, ref SHCOLUMNINFO columnInfo)
        {
            Debug.WriteLine("[{0}] ColumnProviderBase.IColumnProvider.GetColumnInfo, index={1}", Id, index);

            if (!initialized)
                throw new COMException("Initialize was not called");

            //if (m_desktopbugworkaround() || dwIndex >= m_columninfos.size())
            //    return S_FALSE; // tell the shell there are no more columns.

            //*psci = m_columninfos[dwIndex];
            //return S_OK;
        }

        void IColumnProvider.GetItemData(ref SHCOLUMNID columnId, ref SHCOLUMNDATA columnData /*,  VARIANT* pvarData */)
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

        protected abstract void InitializeCore(string folderName);
    }
}
