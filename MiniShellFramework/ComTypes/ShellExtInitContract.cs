// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices.ComTypes;

namespace MiniShellFramework.ComTypes
{
    [ContractClassFor(typeof(IShellExtInit))]
    internal abstract class ShellExtInitContract : IShellExtInit
    {
        public void Initialize(IntPtr pidlFolder, IDataObject dataObject, uint hkeyProgId)
        {
            Contract.Requires(dataObject != null);
        }
    }
}
