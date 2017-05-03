**Project Description**
Extension Framework to write Shell Extensions in .NET 4.0. Please read background info about limitations.

**Background info**
In the past years Microsoft has communicated different signals if developing shell extensions in .NET 4.0 is supported. Initially an article in MSDN seemed to support this: [http://msdn.microsoft.com/en-us/magazine/ee819091.aspx](http://msdn.microsoft.com/en-us/magazine/ee819091.aspx). Later Microsoft provided base classes and samples in the Windows API Code Pack 1.1.; for a thumbnail provider and preview handler.
The latest guidance is however that creating shell extensions in a managed language (Java, .NET, Javascript) is not a supported scenario [http://msdn.microsoft.com/en-us/library/dd758089.aspx](http://msdn.microsoft.com/en-us/library/dd758089.aspx) except for Preview handlers and command-line based action extensions.
The main problem of .NET shell extensions is that these extensions are not only loaded into the shell process (explorer.exe in general) but also in every process that uses the open file \ save common dialogboxes. This can create all kind of support problems. Please see the Microsoft guidance article for a complete overview.
This project is still supported as people may want to use .NET for quick prototyping and/or only release their shell extension only in a controlled environment. MSF is alternative framework in C++.

**Layers**
The projects includes the following 'layers':
* ComType Layer. Managed definition of the COM interfaces used by Shell to interact with shell extensions. The interface definitions are hand tuned  for shell extensions development.
* Base Layer. Set of base classes that can be used as starting point for shell extensions.
* Sample Layer. Set of samples, demonstrating typical shell extensions.
* Start Layer: Skeleton code that can be used as start for a new shell extension.

**Supported Shell Extensions**
* Info Tip (IQueryInfo)
* Shell PropertySheet Extension (IShellPropSheetExt) **{planned}**
* Context Menu Extension (IContextMenu, IContextMenu2, IContextMenu3)
* Folder Copy Hook Extension (ICopyHook)
* Extract Image Extension (IExtractImage, IExtractImage2)**{planned}**
* Column Provider (IColumnProvider), XP only, Vista and up doesn't use column providers.**{planned}**
* Shell Folder (IShellView, IShellFolder)**{planned}**

**Supported Shell Extensions by the Windows API Code Pack 1.1.**
The following 2 shell extensions are supported by the Windows API Code Pack and currently not included in the MMSF project:
* ThumbnailProviders (IThumbnailProvider)
* PreviewHandler (IPreviewHandler): Note: a preview handler can be created in .NET as this extension is hosted out-process.

**Supported Operating Systems**
* Windows 8
* Windows 7 (on demand)
* Windows Vista (on demand)
* Windows XP (on demand)

**Supported Compilers**
* Visual Studio 2012 Update 2
* Visual Studio 2010 SP1

Note: Source Code snapshots are only validated on Windows 8.
Note2: (on demand) means that OS is supported, but not tested. Reported issues will be resolved on demand.