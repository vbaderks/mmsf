// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics.Contracts;

namespace MiniShellFramework.ComTypes
{
    [ContractClassFor(typeof(ICopyHook))]
    internal abstract class CopyHookContract : ICopyHook
    {
        public uint CopyCallback(IntPtr parentWindow, FileOperation fileOperation, uint flags, string source, uint sourceAttributes, string destination, uint destinationAttributes)
        {
            Contract.Requires(source != null);
            return default(uint);
        }
    }
}
