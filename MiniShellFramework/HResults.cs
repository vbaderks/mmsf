// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

namespace MiniShellFramework
{
    /// <summary>
    /// Provides constants and static methods to create HResult values.
    /// </summary>
    public static class HResults
    {
        /// <summary>
        /// An attempt was made to perform an initialization operation when initialization
        /// has already been completed. (HRESULT_FROM_WIN32(ERROR_ALREADY_INITIALIZED)
        /// </summary>
        public const int ErrorAlreadyInitialized = 300; // TODO: HRESULT_FROM_WIN32(ERROR_ALREADY_INITIALIZED)

        /// <summary>
        /// Operation completed ok (S_OK).
        /// </summary>
        public const int Ok = 0;

        /// <summary>
        /// Operation completed without error, result is false (S_FALSE).
        /// </summary>
        public const int False = 1;

        /// <summary>
        /// E_FAIL
        /// </summary>
        public const int ErrorFail = unchecked((int)0x80004005);

        /// <summary>
        /// Creates the specified HResult value with facility none.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static int Create(Severity severity, int code)
        {
            return Create(severity, Facility.None, code);
        }

        /// <summary>
        /// Creates the specified HResult value.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <param name="facility">The facility.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static int Create(Severity severity, Facility facility, int code)
        {
            return ((int)severity << 31) | ((int)facility << 16) | code;
        }
    }
}
