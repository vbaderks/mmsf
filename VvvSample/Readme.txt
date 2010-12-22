 VVV Sample
==========

The VVV Sample Shell extension demonstrates the following types of shell extensions:

- Infotip
- Folder Copyhook
- Column Provider (only on Windows XP, deprecated in Windows Vista and newer) [under development]
- Shortcut menu [under development]

Registering the handlers
------------------------
Shell extensions are in-process COM objects. Before the shell (explorer.exe) can use them they need to be
registered in the registry. This can be done with the following command:

regasm VvvSample.dll /codebase.

regasm.exe is a tool shipped with the the .NET runtime. The option /codebase is needed to ensure the 
current file location is used. /codebase can only be omitted when the shell extension assembly is 
installed in the GAC. 

x64 note: 64 bit versions of Windows run a 64 bit version of explorer.exe. On a x64 bit machines 2 versions
of the .NET runtime are installed: 32 bit and 64 bit. The x64 regasm version needs to be used to register
shell extensions. The x64 bit version of regasm can typically be found in c:\windows\microsoft.NET\Framework64\..

Folder Copyhook
---------------
The folder Copyhook extension will watch folder deletes that have the sub string "VVV-MMSF" in their name.
If that is the case it will display a messagebox to confirm the folder delete operation.
