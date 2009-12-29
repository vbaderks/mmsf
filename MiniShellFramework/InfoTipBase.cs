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
    }

    public abstract class InfoTipBase : IInitializeWithStream, IQueryInfo
    {
        private bool initialized;

        protected InfoTipBase()
        {
            Debug.WriteLine("InfoTipBase::Constructor (instance={0})", this);
        }

        public void Initialize(IStream stream, int grfMode)
        {
            Debug.WriteLine("InfoTipBase::Initialize (withstream) (instance={0}, mode={1})", this, grfMode);
            if (initialized)
                throw new COMException("Already initialized", HResults.ErrorAlreadyInitialized);

            using (var comStream = new ComStream(stream))
            {
                InitializeCore(comStream);
                initialized = true;
            }
        }

        public void GetInfoTip(int dwFlags, out string ppwszTip)
        {
            Debug.WriteLine("InfoTipBase::GetInfoTip (withfile) (instance={0}, dwFlags={1})", this, dwFlags);
            ppwszTip = GetInfoTipCore();
        }

        public void GetInfoFlags(int pdwFlags)
        {
            throw new NotImplementedException();
        }

        protected static void ComRegisterFunction(Type type, string description, string progId)
        {
            // Register the InfoTip COM object as an approved Shell extension. Explorer will only execute approved extensions.
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

        abstract protected void InitializeCore(Stream stream);

        abstract protected string GetInfoTipCore();
    }
}
