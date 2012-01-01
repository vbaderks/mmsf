// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using MiniShellFramework;

namespace VvvSample
{
    [ComVisible(true)]                              // Make this .NET class a COM object (ComVisible is false on assembly level).
    [Guid("AF8D86A2-AC92-4CDB-9105-C3EAC720E214")]  // Explicitly assign a GUID: easier to reference and to debug.
    [ClassInterface(ClassInterfaceType.None)]       // Only the functions from the COM interfaces should be accessible.
    public sealed class ThumbnailProvider : ThumbnailProviderBase
    {
        public ThumbnailProvider() : base(ThumbnailProviderInitializationOptions.Stream)
        {
        }

        [ComRegisterFunction]
        public static void ComRegisterFunction(Type type)
        {
            Contract.Requires(type != null);

            VvvRootKey.Register();
            ComRegister(type, "VVV Sample ShellExtension (InfoTip)", VvvRootKey.ProgId);
        }

        [ComUnregisterFunction]
        public static void ComUnregisterFunction(Type type)
        {
            Contract.Requires(type != null);

            VvvRootKey.Unregister();
            ComUnregister(type, VvvRootKey.ProgId);
        }

        protected override Bitmap GetThumbnail(Stream source, int squareLength)
        {
            var bitmap = new Bitmap(squareLength, squareLength, PixelFormat.Format32bppArgb);
            bitmap.SetPixel(0, 0, SystemColors.ControlDark);
            return bitmap;
        }
    }
}
