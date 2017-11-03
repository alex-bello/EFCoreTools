using System;

namespace EFCoreTools.Security
{
    public class IAppUser
    {
        /// <summary>
        /// The user's Id
        /// </summary>
        string Id { get; set; }
        
        /// <summary>
        /// The user's UserName
        /// </summary>
        string UserName { get; set; }
    }
}