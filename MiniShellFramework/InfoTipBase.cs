// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Win32;
using MiniShellFramework.ComTypes;

namespace MiniShellFramework
{
    /// <summary>
    /// Base class for Infotip shell extension handlers.
    /// </summary>
    [ComVisible(true)]                        // Make this .NET class visible to ensure derived class can be COM visible.
    [ClassInterface(ClassInterfaceType.None)] // Only the functions from the COM interfaces should be accessible.
    public abstract class InfoTipBase : IInitializeWithStream, IQueryInfo, IPersistFile
    {
        private bool initialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="InfoTipBase"/> class.
        /// </summary>
        protected InfoTipBase()
        {
            Debug.WriteLine("{0}.Constructor (InfoTipBase)", this);
        }

        void IInitializeWithStream.Initialize(IStream stream, StorageModes storageMode)
        {
            Debug.WriteLine("{0}.IInitializeWithStream.Initialize (InfoTipBase), mode={1})", this, storageMode);
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
            Debug.WriteLine("{0}.IQueryInfo.GetInfoTip (InfoTipBase), dwFlags={1}", this, options);
            text = GetInfoTipCore();
        }

        void IQueryInfo.GetInfoFlags(out InfoTipOptions options)
        {
            Debug.WriteLine("{0}.IQueryInfo.GetInfoFlags (InfoTipBase) - Not Implemented (functionality not used)", this);
            throw new NotSupportedException();
        }

        void IPersistFile.GetClassID(out Guid pClassID)
        {
            Debug.WriteLine("{0}.IQueryInfo.InfoTipBase.GetClassID (InfoTipBase) - Not Implemented (functionality not used)", this);
            throw new NotSupportedException();
        }

        void IPersistFile.GetCurFile(out string ppszFileName)
        {
            Debug.WriteLine("{0}.IQueryInfo.GetCurFile (InfoTipBase) - Not Implemented (functionality not used)", this);
            throw new NotSupportedException();
        }

        int IPersistFile.IsDirty()
        {
            Debug.WriteLine("{0}.IPersistFile.IsDirty (InfoTipBase) - Not Implemented (functionality not used)", this);
            throw new NotSupportedException();
        }

        void IPersistFile.Load(string pszFileName, int dwMode)
        {
            Debug.WriteLine("{0}.IPersistFile.Load (InfoTipBase), dwMode={1}", this, dwMode);

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
            Debug.WriteLine("{0}.IPersistFile.Save (InfoTipBase) - Not Implemented (functionality not used)", this);
            throw new NotSupportedException();
        }

        void IPersistFile.SaveCompleted(string pszFileName)
        {
            Debug.WriteLine("{0}.IPersistFile.SaveCompleted (InfoTipBase) - Not Implemented (functionality not used)", this);
            throw new NotSupportedException();
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

        /// <summary>
        /// When overridden in a derived class initializes the extension.
        /// </summary>
        /// <param name="stream">The stream the extension should compute the infotip text for.</param>
        protected abstract void InitializeCore(Stream stream);

        /// <summary>
        /// When overridden in a derived class initializes the extension.
        /// </summary>
        /// <returns>a string that can be used as text for a infotip.</returns>
        protected abstract string GetInfoTipCore();
    }
}
