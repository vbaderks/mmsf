using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MiniShellFramework.Interfaces
{
    [ComImport(), Guid("00021500-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IQueryInfo
    {
        void GetInfoTip([In] int dwFlags, [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppwszTip);
        void GetInfoFlags([Out] int pdwFlags);
    }
}
