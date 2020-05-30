# Project Description

Extension Framework to write Microsoft Windows Shell Extensions in .NET 4.x. Please read the Background Information section about the limitations.

## Background Information

In the past years Microsoft has communicated different signals if developing shell extensions in .NET 4.x is supported or not. Initially an article in MSDN seemed to support this: [http://msdn.microsoft.com/en-us/magazine/ee819091.aspx](http://msdn.microsoft.com/en-us/magazine/ee819091.aspx). Later Microsoft provided base classes and samples in the Windows API Code Pack 1.1, for a thumbnail provider and preview handler.
The latest official Microsoft guidance is however that creating Shell extensions in a managed language (Java, .NET, Javascript) is not a supported scenario [http://msdn.microsoft.com/en-us/library/dd758089.aspx](http://msdn.microsoft.com/en-us/library/dd758089.aspx) except for Preview handlers and command-line based action extensions.  
The main problem of Windows Shell extensions is that these extensions are not only loaded into the Windows shell process (explorer.exe in general) but also in every process that uses the open file-save common dialogboxes. This can create all kind of support problems. Please see the Microsoft guidance article for a complete overview.
This project is still supported as people may want to use .NET for quick prototyping and/or only release their shell extension only in a controlled environment. MSF is alternative shell extension framework in C++.

## Layers

The projects includes the following 'layers':

* ComType Layer. Managed definition of the COM interfaces used by Shell to interact with shell extensions. The interface definitions are hand tuned  for shell extensions development.
* Base Layer. Set of base classes that can be used as starting point for shell extensions.
* Sample Layer. Set of samples, demonstrating typical shell extensions.
* Start Layer: Skeleton code that can be used as start for a new shell extension.

## Supported Shell Extensions

* Info Tip (IQueryInfo)
* Shell PropertySheet Extension (IShellPropSheetExt) **{planned}**
* Context Menu Extension (IContextMenu, IContextMenu2, IContextMenu3)
* Folder Copy Hook Extension (ICopyHook)
* Extract Image Extension (IExtractImage, IExtractImage2)**{planned}**
* Shell Folder (IShellView, IShellFolder)**{planned}**

## Supported Shell Extensions by the Windows API Code Pack 1.1

The following 2 shell extensions are supported by the Windows API Code Pack and currently not included in the MMSF project:

* ThumbnailProviders (IThumbnailProvider)
* PreviewHandler (IPreviewHandler): Note: a preview handler can be created in .NET as this extension is hosted out-process.

## Supported Operating Systems

* Windows 10

## Supported Compilers

* Visual Studio 2019

Note: Source Code snapshots are only validated on Windows 10.