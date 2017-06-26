using EFCoreTools.Core.Conventions;
using Microsoft.EntityFrameworkCore;

namespace EFCoreTools.Tests.Models
{
    public class TestDbContext : DbContext
    {
        public TestDbContext() { }

        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {

        }

        public DbSet<Person> Person { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            new IndexConvention().Apply(builder);
            base.OnModelCreating(builder);
        }
        
    }
}
