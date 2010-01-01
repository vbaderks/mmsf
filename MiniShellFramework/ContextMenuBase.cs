// <copyright company="Victor Derks">
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using MiniShellFramework.Interfaces;

namespace MiniShellFramework
{
    public abstract class ContextMenuBase : IShellExtInit, IContextMenu3
    {
        private uint idCmdFirst;

        protected ContextMenuBase()
        {
            Debug.WriteLine("ContextMenuBase::Constructor (instance={0})", this);
        }

        public void Initialize(IntPtr pidlFolder, System.Runtime.InteropServices.ComTypes.IDataObject dataObject, uint hkeyProgId)
        {
            throw new NotImplementedException();
        }

        public int QueryContextMenu(IntPtr hmenu, uint indexMenu, uint idCmdFirst, uint idCmdLast, QueryContextMenuOptions flags)
        {
            Debug.WriteLine("ContextMenuBase::QueryContextMenu (instance={0}, indexMenu={1}, idCmdFirst={2}, idLast={3}, flag={4})",
                this, indexMenu, idCmdFirst, idCmdLast, flags);

            // If the flags include CMF_DEFAULTONLY then we shouldn't do anything.
            if (flags.HasFlag(QueryContextMenuOptions.DefaultOnly))
                return HResults.Create(Severity.Success, 0);

            ////ClearMenuItems();

            this.idCmdFirst = idCmdFirst;
            uint id = idCmdFirst;
            ////CMenu menu(hmenu, indexMenu, nID, idCmdLast, this);

            ////static_cast<T*>(this)->OnQueryContextMenu(menu, GetFilenames());

            return HResults.Create(Severity.Success, (ushort) (id - idCmdFirst));
        }

        public void InvokeCommand(ref InvokeCommandInfo invokeCommandInfo)
        {
            Debug.WriteLine("ContextMenuBase::InvokeCommand (instance={0})", this);

            ////if (HIWORD(pici->lpVerb) != 0)
            if (invokeCommandInfo.lpVerb.ToInt32() != 0)
                throw new ArgumentException("Verbs not supported");

            ////GetMenuItem(LOWORD(pici->lpVerb)).GetContextCommand()(pici, GetFilenames());
        }

        public void InvokeCommand(IntPtr pici)
        {
            throw new NotImplementedException();
        }

        public void GetCommandString(int idCmd, uint uflags, int reserved, StringBuilder commandString, int cch)
        {
            throw new NotImplementedException();
        }

        public int HandleMenuMsg(uint uMsg, uint wParam, uint lParam)
        {
            throw new NotImplementedException();
        }

        public int HandleMenuMsg2(uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr plResult)
        {
            throw new NotImplementedException();
        }

        protected abstract void QueryContextMenuCore();
    }
}
