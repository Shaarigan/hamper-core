// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Threading
{
    /// <summary>
    /// An abstract access handle managed by the corresponding <see cref="AccessManager"/>
    /// </summary>
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    interface IAccessHandle
    {
        /// <summary>
        /// Gets the permission related to the current access of the provided object
        /// </summary>
        /// <typeparam name="T">An object this handle is managing</typeparam>
        /// <returns>The corresponding permission</returns>
        public AccessType GetAccess<T>()
            where T : class;
    }
}