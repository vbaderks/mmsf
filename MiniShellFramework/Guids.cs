// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Collections.Generic;

namespace MiniShellFramework
{
    /// <summary>
    /// TODO
    /// </summary>
    public class Guids
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid IMarshal = new Guid("00000003-0000-0000-C000-000000000046");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid IShellExtInit = new Guid("000214E8-0000-0000-c000-000000000046");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid IPersistFile = new Guid("0000010b-0000-0000-c000-000000000046");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid IContextMenu = new Guid("000214e4-0000-0000-c000-000000000046");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid IQueryInfo = new Guid("00021500-0000-0000-c000-000000000046");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid IObjectWithSite = new Guid("fc4801a3-2ba9-11cf-a229-00aa003d7352");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid IPreviewHandlerGuid = new Guid("8895b1c6-b41f-4c1c-a562-0d564250836f");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid IThumbnailProviderGuid = new Guid("e357fccd-a995-4576-b01f-234630154e96");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid IInitializeWithFileGuid = new Guid("b7d14566-0509-4cce-a71f-0a554233bd9b");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid IInitializeWithStreamGuid = new Guid("b824b49d-22ac-4161-ac8a-9916e8fa3f7f");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid IInitializeWithItemGuid = new Guid("7f73be3f-fb79-493c-a6c7-7ee14e245841");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid IInternetSecurityManager = new Guid("79eac9ee-baf9-11ce-8c82-00aa004ba90b");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid ShellPropSheetExtIId = new Guid("000214E9-0000-0000-C000-000000000046");

        // QueryInterface seen for ContextMenu on Windows 7 x64.
        internal static readonly Guid Undocumented1 = new Guid("924502A7-CC8E-4F60-AE1F-F70C0A2B7A7C");

        private static readonly IDictionary<Guid, string> names = CreateNameDictionary();

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static bool TryGetName(Guid key, out string name)
        {
            return names.TryGetValue(key, out name);
        }

        private static IDictionary<Guid, string> CreateNameDictionary()
        {
            var results = new Dictionary<Guid, string>();

            results.Add(IMarshal, "IMarshal");
            results.Add(IShellExtInit, "IShellExtInit");
            results.Add(IPersistFile, "IPersistFile");
            results.Add(IQueryInfo, "IQueryInfo");
            results.Add(IObjectWithSite, "IObjectWithSite");
            results.Add(IPreviewHandlerGuid, "IPreviewHandlerGuid");
            results.Add(IInternetSecurityManager, "IInternetSecurityManager");
            results.Add(Undocumented1, "IUndocumented1");
            results.Add(ShellPropSheetExtIId, "IShellPropSheetExt");

            return results;
        }
    }
}
