using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniShellFramework.Interfaces;

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

    public class VvvFile
    {
        public VvvFile(string path)
        {
        }

        public string Label { get; set; }

        public int FileCount { get; set; }
    }


    public class InfoTip : IInitializeWithFile, IQueryInfo
    {
        private VvvFile vvvFile;

        public void Initialize(string filePath, int grfMode)
        {
            if (vvvFile != null)

            vvvFile = new VvvFile(filePath);
        }

        public void GetInfoTip(int dwFlags, out string ppwszTip)
        {
            ppwszTip = string.Format("Label: {0}\nFile count: {1}", vvvFile.Label, vvvFile.FileCount);
        }

        public void GetInfoFlags(int pdwFlags)
        {
            throw new NotImplementedException();
        }
    }


}
