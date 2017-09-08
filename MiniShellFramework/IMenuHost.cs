// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

namespace MiniShellFramework
{
    internal interface IMenuHost
    {
        uint GetCommandId();

        void IncrementCommandId();

        void OnAddMenuItem(string helpText, Menu.ContextCommand contextCommand, CustomMenuHandler customMenuHandler);
    }
}
