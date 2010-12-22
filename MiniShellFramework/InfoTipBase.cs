// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
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
    public abstract class InfoTipBase : ShellExtension, IInitializeWithStream, IQueryInfo, IPersistFile
    {
        private bool initialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="InfoTipBase"/> class.
        /// </summary>
        protected InfoTipBase()
        {
            Debug.WriteLine("[{0}] InfoTipBase.Constructor", Id);
        }

        void IInitializeWithStream.Initialize(IStream stream, StorageModes storageMode)
        {
            Debug.WriteLine("[{0}] InfoTipBase.IInitializeWithStream.Initialize, mode={1})", Id, storageMode);

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
            Debug.WriteLine("[{0}] InfoTipBase.IQueryInfo.GetInfoTip, dwFlags={1}", Id, options);
            text = GetInfoTipCore();
        }

        void IQueryInfo.GetInfoFlags(out InfoTipOptions options)
        {
            Debug.WriteLine("[{0}] InfoTipBase.IQueryInfo.GetInfoFlags - Not Implemented (functionality not used)", Id);
            throw new NotSupportedException();
        }

        void IPersistFile.GetClassID(out Guid pClassID)
        {
            Debug.WriteLine("[{0}] InfoTipBase.IQueryInfo.InfoTipBase.GetClassID - Not Implemented (functionality not used)", Id);
            throw new NotSupportedException();
        }

        void IPersistFile.GetCurFile(out string ppszFileName)
        {
            Debug.WriteLine("[{0}] InfoTipBase.IQueryInfo.GetCurFile - Not Implemented (functionality not used)", Id);
            throw new NotSupportedException();
        }

        int IPersistFile.IsDirty()
        {
            Debug.WriteLine("[{0}] InfoTipBase.IPersistFile.IsDirty - Not Implemented (functionality not used)", Id);
            throw new NotSupportedException();
        }

        void IPersistFile.Load(string pszFileName, int dwMode)
        {
            Debug.WriteLine("[{0}] InfoTipBase.IPersistFile.Load, dwMode={1}", Id, dwMode);

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
            Debug.WriteLine("[{0}] InfoTipBase.IPersistFile.Save - Not Implemented (functionality not used)", Id);
            throw new NotSupportedException();
        }

        void IPersistFile.SaveCompleted(string pszFileName)
        {
            Debug.WriteLine("[{0}] InfoTipBase.IPersistFile.SaveCompleted - Not Implemented (functionality not used)", Id);
            throw new NotSupportedException();
        }

        /// <summary>
        /// Adds additional info to the registry to allow the shell to discover the oject as shell extension.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="description">The description.</param>
        /// <param name="progId">The prog id.</param>
        protected static void ComRegister(Type type, string description, string progId)
        {
            Contract.Requires(type != null);
            Contract.Requires(!string.IsNullOrEmpty(description));
            Contract.Requires(!string.IsNullOrEmpty(progId));

            RegistryExtensions.AddAsApprovedShellExtension(type, description);

            // Register the InfoTip COM object as the InfoTip handler. Only 1 handler can be installed for a file type.
            var subKeyName = progId + @"\ShellEx\{00021500-0000-0000-C000-000000000046}";
            using (var key = Registry.ClassesRoot.CreateSubKey(subKeyName))
            {
                if (key == null)
                    throw new ApplicationException("Failed to create registry key: " + subKeyName);

                key.SetValue(string.Empty, type.GUID.ToString("B"));
            }
        }

        /// <summary>
        /// Removed the additional info from the registry that allowed the shell to discover the shell extension.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="progId">The prog id.</param>
        protected static void ComUnregister(Type type, string progId)
        {
            Contract.Requires(type != null);

            RegistryExtensions.RemoveAsApprovedShellExtension(type);
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
