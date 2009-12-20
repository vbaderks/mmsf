using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniShellFramework.Interfaces;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace MiniShellFramework
{
    public abstract class InfoTipBase : IInitializeWithFile, IQueryInfo
    {
        bool initialized;

        public void Initialize(string filePath, int grfMode)
        {
            Debug.WriteLine("InfoTipCore::Initialize (withfile) (instance={0}, mode={1}, filename={2})", this, grfMode, filePath);

            if (initialized)
                throw new COMException("Already initialized", HResults.ErrorAlreadyInitialized);

            InitializeCore();
            initialized = true;
        }

        public void GetInfoTip(int dwFlags, out string ppwszTip)
        {
            Debug.WriteLine("InfoTipCore::Initialize (withfile) (instance={0}, dwFlags={1})", this, dwFlags);
            throw new NotImplementedException();
        }

        public void GetInfoFlags(int pdwFlags)
        {
            throw new NotImplementedException();
        }

        abstract protected void InitializeCore();

        abstract protected void GetInfoTipCore();
    }
}
