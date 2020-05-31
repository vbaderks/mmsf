﻿// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniShellFramework.ComTypes;

namespace VvvSampleTest
{
    /// <summary>
    /// This is a test class for FolderCopyHookTest and is intended
    /// to contain all FolderCopyHookTest Unit Tests
    /// </summary>
    [TestClass]
    public class FolderCopyHookTest
    {
        [TestMethod]
        public void FolderCopyHookCreate()
        {
            var folderCopyHook = CreateFolderCopyHook();
            Assert.IsNotNull(folderCopyHook);
            Assert.IsNotNull(folderCopyHook as ICopyHook);
        }

        [TestMethod]
        public void DeleteNormalFolder()
        {
            var folderCopyHook = (ICopyHook)CreateFolderCopyHook();
            uint result = folderCopyHook.CopyCallback(new IntPtr(), FileOperation.Delete, 0, "bla", 0, "boe", 0);
            Assert.AreEqual(6U, result);
        }

        [TestMethod]
        [Ignore] // Unittest will display GUI. Cannot be executed in batch. (mstest doesn't known the Explicit attribute)
        public void DeleteProtectedFolder()
        {
            var folderCopyHook = (ICopyHook)CreateFolderCopyHook();
            folderCopyHook.CopyCallback(new IntPtr(), FileOperation.Delete, 0, "blaVVV", 0, "boe", 0);
        }

        private static object CreateFolderCopyHook()
        {
            return Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("5070BD33-0BD4-4B4C-B5C6-9E09FCFD6DD2")));
        }
    }
}
