using System;
using System.Collections.Generic;

namespace EFCoreTools
{
    /// <summary>
    /// Represents an entity in the data store with an int type used for the primary key.
    /// </summary>
    public abstract class Entity : Entity<int> 
    {

    }
    
    /// <summary>
    /// Represents an entity in the data store.
    /// </summary
    /// <typeparam name="TKey">The type used for the primary key for the user.</typeparam>
    public abstract class Entity<TKey> 
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// The primary key of the entity object.
        /// </summary>
        public TKey Id { get; set; }
    }
}