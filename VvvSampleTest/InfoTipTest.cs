using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MiniShellFramework.Interfaces;
using System.Runtime.InteropServices;

namespace VvvSampleTest
{
    
    
    /// <summary>
    ///This is a test class for InfoTipTest and is intended
    ///to contain all InfoTipTest Unit Tests
    ///</summary>
    [TestClass()]
    public class InfoTipTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        public void InfoTipCreate()
        {
            object infoTip = CreateInfoTip();
            Assert.IsNotNull(infoTip);
            Assert.IsNotNull((IQueryInfo)infoTip);

            //Marshal.ReleaseComObject(infoTip);
        }

        [TestMethod]
        public void FileNotFound()
        {
            var infoTip = (IInitializeWithFile)CreateInfoTip();
            infoTip.Initialize("filenotfound.vvv", 0);

            // TODO: should throw an exception.
        }

        private object CreateInfoTip()
        {
            return Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("EDD37CEF-F1E0-42bb-9AEF-177E0306AA71")));
        }
    }
}
