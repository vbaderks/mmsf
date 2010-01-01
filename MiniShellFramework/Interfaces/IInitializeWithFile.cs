// <copyright company="Victor Derks">
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MiniShellFramework.Interfaces
{
    [Flags]
    public enum StorageModes
    {
        Read      = 0x0,
        ReadWrite = 0x2
    }

    public enum Severity
    {
        Success = 0,
        Error   = 1
    }

    public enum Facility
    {
        None = 0,     // FACILITY_NULL
        Interface = 4 // FACILITY_ITF
    }

    public static class HResults
    {
        public const int ErrorAlreadyInitialized = 300; //HRESULT_FROM_WIN32(ERROR_ALREADY_INITIALIZED)

        public static int Create(Severity severity, int code)
        {
            return Create(severity, Facility.None, code);
        }

        public static int Create(Severity severity, Facility facility, int code)
        {
            return ((int)severity << 31) | ((int)facility << 16) | code;
        }
    }

    [ComImport(), Guid("b7d14566-0509-4cce-a71f-0a554233bd9b"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IInitializeWithFile
    {
        void Initialize(string filePath, int grfMode);
    }
}
