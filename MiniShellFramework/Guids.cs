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
        public static readonly Guid MarshalInterfaceId = new Guid("00000003-0000-0000-C000-000000000046");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid ShellExtInitInterfaceId = new Guid("000214E8-0000-0000-c000-000000000046");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid PersistFileInterfaceId = new Guid("0000010b-0000-0000-c000-000000000046");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid ContextMenuInterfaceId = new Guid("000214e4-0000-0000-c000-000000000046");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid ContextMenu2InterfaceId = new Guid("000214f4-0000-0000-c000-000000000046");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid ContextMenu3InterfaceId = new Guid("bcfce0a0-ec17-11d0-8d10-00a0c90f2719");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid QueryInfoInterfaceId = new Guid("00021500-0000-0000-c000-000000000046");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid ObjectWithSiteInterfaceId = new Guid("fc4801a3-2ba9-11cf-a229-00aa003d7352");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid PreviewHandlerGuidInterfaceId = new Guid("8895b1c6-b41f-4c1c-a562-0d564250836f");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid ThumbnailProviderInterfaceId = new Guid("e357fccd-a995-4576-b01f-234630154e96");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid InitializeWithFileInterfaceId = new Guid("b7d14566-0509-4cce-a71f-0a554233bd9b");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid InitializeWithStreamInterfaceId = new Guid("b824b49d-22ac-4161-ac8a-9916e8fa3f7f");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid InitializeWithItemInterfaceId = new Guid("7f73be3f-fb79-493c-a6c7-7ee14e245841");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid InternetSecurityManagerInterfaceId = new Guid("79eac9ee-baf9-11ce-8c82-00aa004ba90b");

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid ShellPropSheetExtInterfaceId = new Guid("000214E9-0000-0000-C000-000000000046");

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
            return new Dictionary<Guid, string>
                              {
                                  { MarshalInterfaceId, "IMarshal" },
                                  { ShellExtInitInterfaceId, "IShellExtInit" },
                                  { PersistFileInterfaceId, "IPersistFile " },
                                  { QueryInfoInterfaceId, "IQueryInfo" },
                                  { ObjectWithSiteInterfaceId, "IObjectWithSite" },
                                  { PreviewHandlerGuidInterfaceId, "IPreviewHandlerGuid" },
                                  { InternetSecurityManagerInterfaceId, "IInternetSecurityManager" },
                                  { Undocumented1, "IUndocumented1" },
                                  { ContextMenuInterfaceId, "IContextMenu" },
                                  { ContextMenu2InterfaceId, "IContextMenu2" },
                                  { ContextMenu3InterfaceId, "IContextMenu3" },
                                  { ShellPropSheetExtInterfaceId, "IShellPropSheetExt" }
                              };
        }
    }
}
