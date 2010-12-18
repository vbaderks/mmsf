// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.InteropServices;
using MiniShellFramework;

namespace VvvSample
{
    [ComVisible(true)]                              // Make this .NET class a COM object (ComVisible is false on assembly level).
    [Guid("6FA9260E-F61C-48C0-A0E8-0297C8FAE0D8")]  // Explicitly assign a GUID: easier to reference and to debug.
    [ClassInterface(ClassInterfaceType.None)]       // Only the functions from the COM interfaces should be accessible.
    public class InfoTip : InfoTipBase
    {
        private VvvFile vvvFile;

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

        protected override void InitializeCore(Stream stream)
        {
            vvvFile = new VvvFile(stream);
        }

        protected override string GetInfoTipCore()
        {
            Contract.Assume(vvvFile != null);
            return string.Format("Label: {0}\nFile count: {1}", vvvFile.Label, vvvFile.FileCount);
        }
    }
}
