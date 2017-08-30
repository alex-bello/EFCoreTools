using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFCoreTools.Core
{
    public interface IDbContext
    {
         ChangeTracker ChangeTracker { get; }
    }
}