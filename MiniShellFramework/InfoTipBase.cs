// <copyright company="Victor Derks">
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using MiniShellFramework.Interfaces;
using System.Runtime.InteropServices.ComTypes;
using System.IO;

namespace MiniShellFramework
{
    public static class RootKey
    {
        public static void Register(string fileExtension, string progId)
        {
            using (var key = Registry.ClassesRoot.CreateSubKey(fileExtension))
            {
                key.SetValue(string.Empty, progId);
            }
        }

        public static void Unregister(string fileExtension)
        {
            Registry.ClassesRoot.DeleteSubKey(fileExtension);
        }
    }

    public abstract class InfoTipBase : IInitializeWithStream, IQueryInfo, IPersistFile
    {
        private bool initialized;

        protected InfoTipBase()
        {
            Debug.WriteLine("InfoTipBase::Constructor (instance={0})", this);
        }

        // IInitializeWithStream
        public void Initialize(IStream stream, int grfMode)
        {
            Debug.WriteLine("InfoTipBase::Initialize (IInitializeWithStream) (instance={0}, mode={1})", this, grfMode);
            if (initialized)
                throw new COMException("Already initialized", HResults.ErrorAlreadyInitialized);

            using (var comStream = new ComStream(stream))
            {
                InitializeCore(comStream);
                initialized = true;
            }
        }

        // IQueryInfo
        public void GetInfoTip(int dwFlags, out string ppwszTip)
        {
            Debug.WriteLine("InfoTipBase.GetInfoTip (IQueryInfo) - instance={0}, dwFlags={1}", this, dwFlags);
            ppwszTip = GetInfoTipCore();
        }

        public void GetInfoFlags(int pdwFlags)
        {
            Debug.WriteLine("InfoTipBase.GetInfoFlags (IQueryInfo) - Not Implemented (functionality not used)");
            throw new NotImplementedException();
        }

        // IPersistFile
        public void GetClassID(out Guid pClassID)
        {
            Debug.WriteLine("InfoTipBase.GetClassID (IQueryInfo) - Not Implemented (functionality not used)");
            throw new NotImplementedException();
        }

        public void GetCurFile(out string ppszFileName)
        {
            Debug.WriteLine("InfoTipBase.GetCurFile (IQueryInfo) - Not Implemented (functionality not used)");
            throw new NotImplementedException();
        }

        public int IsDirty()
        {
            Debug.WriteLine("InfoTipBase.IsDirty (IQueryInfo) - Not Implemented (functionality not used)");
            throw new NotImplementedException();
        }

        public void Load(string pszFileName, int dwMode)
        {
            Debug.WriteLine("InfoTipBase::Load (IQueryInfo) (instance={0}, dwMode={1})", this, dwMode);

            if (initialized)
                throw new COMException("Already initialized", HResults.ErrorAlreadyInitialized);

            using (var stream = new FileStream(pszFileName, FileMode.Open, FileAccess.Read))
            {
                InitializeCore(stream);
                initialized = true;
            }
        }

        public void Save(string pszFileName, bool fRemember)
        {
            Debug.WriteLine("InfoTipBase::IsDirty (IQueryInfo) - Not Implemented (functionality not used)");
            throw new NotImplementedException();
        }

        public void SaveCompleted(string pszFileName)
        {
            Debug.WriteLine("InfoTipBase::IsDirty (IQueryInfo) - Not Implemented (functionality not used)");
            throw new NotImplementedException();
        }

        protected static void ComRegisterFunction(Type type, string description, string progId)
        {
            // Register the InfoTip COM object as an approved shell extension. Explorer will only execute approved extensions.
            using (var key =
                Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved", true))
            {
                key.SetValue(type.GUID.ToString("B"), description);
            }

            // Register the InfoTip COM object as the InfoTip handler. Only 1 handler can be installed for a file type.
            using (var key = Registry.ClassesRoot.CreateSubKey(progId + @"\ShellEx\{00021500-0000-0000-C000-000000000046}"))
            {
                key.SetValue(string.Empty, type.GUID.ToString("B"));
            }
        }

        protected static void ComUnregisterFunction(Type type, string progId)
        {
            // Unregister the InfoTip COM object as an approved shell extension.
            using (var key =
                Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved", true))
            {
                key.DeleteValue(type.GUID.ToString("B"));
            }

            // Note: prog id should be removed by 1 global unregister function.
        }

        abstract protected void InitializeCore(Stream stream);

        abstract protected string GetInfoTipCore();
    }
}
