// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System.Runtime.InteropServices;

namespace MiniShellFramework.ComTypes
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00021500-0000-0000-C000-000000000046")]
    public interface IQueryInfo
    {
        void GetInfoTip([In] int dwFlags, [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppwszTip);
        void GetInfoFlags([Out] out int pdwFlags);
    }
}
