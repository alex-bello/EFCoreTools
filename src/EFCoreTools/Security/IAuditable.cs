using System;

namespace EFCoreTools.Core.Security
{
    /// <summary>
    /// Defines an interface for saving changes to an entity, the minimum of which 
    /// is the user that created/modified the record and the timestamp of the 
    /// creation/modification.
    /// </summary>
    public interface IAuditable<TUser> : IAuditable
        where TUser : IAppUser
    {
        TUser CreatedByUser { get; set; }
        TUser ModifiedByUser { get; set; }

        string CreatedByUserId { get; set; }
        DateTime DateCreated { get; set; }
        string ModifiedByUserId { get; set; }
        DateTime DateModified { get; set; }
    }

    /// <summary>
    /// Defines an empty interface for determining which entity types implement
    /// the auditable functionality. This is used in overriding SaveChanges to 
    /// automatically populate the user information to the record prior to saving.
    ///  </summary>
    public interface IAuditable
        
    {
        
    }
}