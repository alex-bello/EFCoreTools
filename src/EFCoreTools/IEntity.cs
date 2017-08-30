using System;

namespace EFCoreTools.Core
{
    public interface IEntity : IEntity<int>
    {
         
    }

    public interface IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        TKey Id { get; set; }
    }
}