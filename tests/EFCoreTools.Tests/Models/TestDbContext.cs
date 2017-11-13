using EFCoreTools.Conventions;
using EFCoreTools.Relational;
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
            
            var tableBuilder = new RelationalTableSettingsBuilder();
            tableBuilder.Add(new RelationalTableSettings(typeof(Person), "admin"));
            tableBuilder.Apply(builder);

            base.OnModelCreating(builder);
        }

        public static TestDbContext New(string inMemoryDbName)
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: inMemoryDbName)
                .Options;

            return new TestDbContext(options);
        }
        
    }
}
