using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VvvSample
{
    internal class VvvFile
    {
        internal VvvFile(Stream stream)
        {
            Label = "LABEL PLACEHOLDER";
            FileCount = 5;
        }

        internal string Label { get; set; }

        internal int FileCount { get; set; }
    }
}
