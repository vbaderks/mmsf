// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System.Diagnostics.Contracts;

namespace MiniShellFramework
{
    [ContractClassFor(typeof(IMenuHost))]
    internal abstract class MenuHostContract : IMenuHost
    {
        public uint GetCommandId()
        {
            return default(uint);
        }

        public void IncrementCommandId()
        {
        }

        public void OnAddMenuItem(string helpText, Menu.ContextCommand contextCommand, CustomMenuHandler customMenuHandler)
        {
            Contract.Requires(helpText != null);
        }
    }
}
