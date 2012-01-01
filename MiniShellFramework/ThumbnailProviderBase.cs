// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using MiniShellFramework.ComTypes;

namespace MiniShellFramework
{
    public enum ThumbnailProviderInitializationOptions
    {
        Stream = 1,

        Item = 2,

        File = 4
    }


    /// <summary>
    /// Base class for thumbnail provider shell extension handlers.
    /// </summary>
    [ComVisible(true)]                        // Make this .NET class COM visible to ensure derived class can be COM visible.
    [ClassInterface(ClassInterfaceType.None)] // Only the functions from the COM interfaces should be accessible.
    public abstract class ThumbnailProviderBase : ShellExtension, IInitializeWithStream, IInitializeWithFile, IThumbnailProvider
    {
        private ThumbnailProviderInitializationOptions initializationOptions;
        private bool initialized;
        private ComStream comStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThumbnailProviderBase"/> class.
        /// </summary>
        /// <param name="initializationOptions">The initialization options.</param>
        protected ThumbnailProviderBase(ThumbnailProviderInitializationOptions initializationOptions)
        {
            this.initializationOptions = initializationOptions;
        }

        void IInitializeWithStream.Initialize(IStream stream, StorageModes storageMode)
        {
            Debug.WriteLine("[{0}] ThumbnailProviderBase.IInitializeWithStream.Initialize, mode={1})", Id, storageMode);

            if (initialized)
                throw new COMException("Already initialized", HResults.ErrorAlreadyInitialized);

            comStream = new ComStream(stream);
            initialized = true;
        }

        void IInitializeWithFile.Initialize(string filePath, StorageModes storageMode)
        {
            throw new NotImplementedException();
        }

        void IThumbnailProvider.GetThumbnail(uint squareLength, out IntPtr bitmapHandle, out ThumbnailAlphaType alphaType)
        {
            Debug.WriteLine("[{0}] ThumbnailProviderBase.IThumbnailProvider.GetThumbnail, squareLength={1})", Id, squareLength);

            if (initialized)
                throw new COMException("Not initialized", HResults.ErrorFail);

            Bitmap thumbnail = null;
            if (comStream != null)
            {
                thumbnail = GetThumbnail(comStream, (int)squareLength);
            }

            bitmapHandle = thumbnail.GetHbitmap();
            thumbnail.Dispose();
            alphaType = ThumbnailAlphaType.Unknown;
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
            //var subKeyName = progId + @"\ShellEx\{00021500-0000-0000-C000-000000000046}";
            //using (var key = Registry.ClassesRoot.CreateSubKey(subKeyName))
            //{
            //    if (key == null)
            //        throw new ApplicationException("Failed to create registry key: " + subKeyName);

            //    key.SetValue(string.Empty, type.GUID.ToString("B"));
            //}
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

        protected override CustomQueryInterfaceResult GetInterfaceCore(ref Guid iid, ref IntPtr ppv)
        {
            if (iid == Guids.InitializeWithStreamInterfaceId)
            {
                return initializationOptions.HasFlag(ThumbnailProviderInitializationOptions.Stream) ?
                    CustomQueryInterfaceResult.NotHandled : CustomQueryInterfaceResult.Failed;
            }

            if (iid == Guids.InitializeWithFileInterfaceId)
            {
                return initializationOptions.HasFlag(ThumbnailProviderInitializationOptions.File) ?
                    CustomQueryInterfaceResult.NotHandled : CustomQueryInterfaceResult.Failed;
            }

            return CustomQueryInterfaceResult.NotHandled;
        }

        protected virtual Bitmap GetThumbnail(Stream source, int squareLength)
        {
            throw new NotImplementedException();
        }
    }
}
