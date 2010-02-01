// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

namespace MiniShellFramework.Interfaces
{
    public enum FileOperation
    {
        //  FO_MOVE                    0x0001
        Move = 1,

        // FO_COPY                    0x0002
        Copy = 2,

        // FO_DELETE                  0x0003
        Delete = 3,

        // FO_RENAME                  0x0004
        Rename = 4
    }
}