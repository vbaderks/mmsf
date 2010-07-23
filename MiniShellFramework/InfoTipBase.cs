// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using MiniShellFramework.ComTypes;
using System.Runtime.InteropServices.ComTypes;
using System.IO;

namespace MiniShellFramework
{
    /// <summary>
    /// Base class for Infotip shell extension handlers.
    /// </summary>
    public abstract class InfoTipBase : IInitializeWithStream, IQueryInfo, IPersistFile
    {
        private bool initialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="InfoTipBase"/> class.
        /// </summary>
        protected InfoTipBase()
        {
            Debug.WriteLine("InfoTipBase::Constructor (instance={0})", this);
        }

        void IInitializeWithStream.Initialize(IStream stream, StorageModes storageMode)
        {
            Debug.WriteLine("InfoTipBase::Initialize (IInitializeWithStream) (instance={0}, mode={1})", this, storageMode);
            if (initialized)
                throw new COMException("Already initialized", HResults.ErrorAlreadyInitialized);

            using (var comStream = new ComStream(stream))
            {
                InitializeCore(comStream);
                initialized = true;
            }
        }

        void IQueryInfo.GetInfoTip(InfoTipOptions options, out string text)
        {
            Debug.WriteLine("InfoTipBase.GetInfoTip (IQueryInfo) - instance={0}, dwFlags={1}", this, options);
            text = GetInfoTipCore();
        }

        void IQueryInfo.GetInfoFlags(out InfoTipOptions options)
        {
            Debug.WriteLine("InfoTipBase.GetInfoFlags (IQueryInfo) - Not Implemented (functionality not used)");
            throw new NotImplementedException();
        }

        void IPersistFile.GetClassID(out Guid pClassID)
        {
            Debug.WriteLine("InfoTipBase.GetClassID (IQueryInfo) - Not Implemented (functionality not used)");
            throw new NotImplementedException();
        }

        void IPersistFile.GetCurFile(out string ppszFileName)
        {
            Debug.WriteLine("InfoTipBase.GetCurFile (IQueryInfo) - Not Implemented (functionality not used)");
            throw new NotImplementedException();
        }

        int IPersistFile.IsDirty()
        {
            Debug.WriteLine("InfoTipBase.IsDirty (IQueryInfo) - Not Implemented (functionality not used)");
            throw new NotImplementedException();
        }

        void IPersistFile.Load(string pszFileName, int dwMode)
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

        void IPersistFile.Save(string pszFileName, bool fRemember)
        {
            Debug.WriteLine("InfoTipBase::IsDirty (IQueryInfo) - Not Implemented (functionality not used)");
            throw new NotImplementedException();
        }

        public void SaveCompleted(string pszFileName)
        {
            Debug.WriteLine("InfoTipBase::IsDirty (IQueryInfo) - Not Implemented (functionality not used)");
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds additional info to the registry to allow the shell to discover the oject as shell extension.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="description">The description.</param>
        /// <param name="progId">The prog id.</param>
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

        /// <summary>
        /// Removed the additional info from the registry that allowed the shell to discover the shell extension.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="progId">The prog id.</param>
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
