//
// (C) Copyright by Victor Derks
//
// See README.TXT for the details of the software licence.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MiniShellFramework.Interfaces
{
    [ComImport()]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00021500-0000-0000-C000-000000000046")]
    public interface IQueryInfo
    {
        void GetInfoTip([In] int dwFlags, [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppwszTip);
        void GetInfoFlags([Out] int pdwFlags);
    }
}
