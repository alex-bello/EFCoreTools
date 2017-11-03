using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFCoreTools
{
    public interface IDbContext
    {
         ChangeTracker ChangeTracker { get; }
    }
}