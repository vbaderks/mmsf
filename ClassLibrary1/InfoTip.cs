using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniShellFramework.Interfaces;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using MiniShellFramework;

namespace ClassLibrary1
{
    [Flags]
    public enum InfoTipOptions
    {
        Default = 0,
        UseName = 1,
        LinkNoTarget = 2,
        LinkUseTarget = 4,
        UseSlowTip = 8,
        SingleLine = 0x10 // only on Vista or higer
    }

    internal static class VvvRootKey
    {
        static bool registered;

        public const string ProgId = "VVVFile";

        public static void Register()
        {
            if (registered)
                return;

            RootKey.Register(".vvv", ProgId);
            registered = true;
        }
    }

    public class VvvFile
    {
        public VvvFile(string path)
        {
            Label = "LABEL PLACEHOLDER";
            FileCount = 5;
        }

        public string Label { get; set; }

        public int FileCount { get; set; }
    }

    [ComVisible(true)]
    [Guid("EDD37CEF-F1E0-42bb-9AEF-177E0306AA71")]
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
        }

        protected override void InitializeCore()
        {
            vvvFile = new VvvFile(null /*filePath*/);
        }

        protected override string GetInfoTipCore()
        {
            return string.Format("Label: {0}\nFile count: {1}", vvvFile.Label, vvvFile.FileCount);
        }
    }
}
