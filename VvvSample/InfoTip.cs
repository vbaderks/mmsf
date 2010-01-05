﻿// <copyright company="Victor Derks">
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniShellFramework.Interfaces;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using MiniShellFramework;
using System.IO;

namespace VvvSample
{
    [ComVisible(true)]                              // Make this .NET class a COM object (ComVisible is false on assembly level).
    [Guid("EDD37CEF-F1E0-42bb-9AEF-177E0306AA71")]  // Explicitly assign a GUID: easier to reference and to debug.
    [ClassInterface(ClassInterfaceType.None)]       // Only the functions from the COM interfaces should be accessible.
    public class InfoTip : InfoTipBase
    {
        private VvvFile vvvFile;

        [ComRegisterFunction]
        public static void ComRegisterFunction(Type type)
        {
            VvvRootKey.Register();
            ComRegisterFunction(type, "VVV Sample ShellExtension (InfoTip)", VvvRootKey.ProgId);
        }

        [ComUnregisterFunction]
        public static void ComUnregisterFunction(Type type)
        {
            ComUnregisterFunction(type);
        }

        protected override void InitializeCore(Stream stream)
        {
            vvvFile = new VvvFile(null /*filePath*/);
        }

        protected override string GetInfoTipCore()
        {
            return string.Format("Label: {0}\nFile count: {1}", vvvFile.Label, vvvFile.FileCount);
        }
    }
}