// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MiniShellFramework.ComTypes;

namespace MiniShellFramework
{
    /// <summary>
    /// Provide a base class for property sheet shell extensions.
    /// </summary>
    [ComVisible(true)]                        // Make this .NET class visible to ensure derived class can be COM visible.
    [ClassInterface(ClassInterfaceType.None)] // Only the functions from the COM interfaces should be accessible.
    public class ColumnProviderBase : IColumnProvider
    {

        // IColumnProvider
        void Initialize(/* const SHCOLUMNINIT* psci */)
        {
            //ATLTRACE2(atlTraceCOM, 0,
            //    _T("IColumnProviderImpl::Initialize, i=%p, tid=%d, dwFlags=%d, wszFolder=%s\n"),
            //    this, GetCurrentThreadId(), psci->dwFlags, psci->wszFolder);

            //// Note: the SDK docs are unclear if 
            //// TODO: check if explorer calls this function twice.

            //// Clear internal caching variables.
            //m_pCachedInfo = NULL;
            //m_strCachedFilename.Empty();
            //m_mapCacheInfo.clear();

            //m_desktopbugworkaround.Initialize(psci->wszFolder);

            //m_columninfos.clear();
            //m_columnidtoindex.clear();

            //// Note: OnInitialize needs to be implemented by the derived class.
            //static_cast<T*>(this)->OnInitialize(psci->wszFolder);

            //m_bInitialized = true;
            //return S_OK;
        }

        // Purpose: GetColumnInfo is called by the shell to retrieve the column names.
        // By calling this repeatedly the shell can detect how many columns there are.
        void GetColumnInfo(/* DWORD dwIndex, SHCOLUMNINFO* psci */)
        {
            //ATLTRACE2(atlTraceCOM, 0,
            //    _T("IColumnProviderImpl::GetColumnInfo, i=%p, tid=%d, dwIndex=%d\n"),
            //    this, GetCurrentThreadId(), dwIndex);

            //if (!m_bInitialized)
            //{
            //    ATLTRACE2(atlTraceCOM, 0,
            //        _T("IColumnProviderImpl::GetColumnInfo, i=%p, Initialize was not called\n"), this);
            //    return E_FAIL;
            //}

            //if (m_desktopbugworkaround() || dwIndex >= m_columninfos.size())
            //    return S_FALSE; // tell the shell there are no more columns.

            //*psci = m_columninfos[dwIndex];
            //return S_OK;
        }

        void GetItemData(/*const SHCOLUMNID* pscid, const SHCOLUMNDATA* pscd, VARIANT* pvarData */)
        {
            //ATLTRACE2(atlTraceCOM, 0, _T("IColumnProviderImpl::GetItemData, i=%p, tid=%d, f=%d, c=%d, file=%s\n"),
            //    this, GetCurrentThreadId(), pscd->dwFlags, pscid->pid, CW2T(pscd->wszFile));

            //if (!m_bInitialized)
            //{
            //    ATLTRACE2(atlTraceCOM, 0,
            //        _T("IColumnProviderImpl::GetItemData, i=%p, Initialize was not called\n"), this);
            //    return E_FAIL;
            //}

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
    }
}
