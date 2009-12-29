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

    public static class HResults
    {
        public const int ErrorAlreadyInitialized = 300; //HRESULT_FROM_WIN32(ERROR_ALREADY_INITIALIZED)
    }

    [ComImport(), Guid("b7d14566-0509-4cce-a71f-0a554233bd9b"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IInitializeWithFile
    {
        void Initialize(string filePath, int grfMode);
    }
}
