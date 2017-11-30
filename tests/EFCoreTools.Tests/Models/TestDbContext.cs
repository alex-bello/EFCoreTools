using EFCoreTools.Conventions;
using EFCoreTools.Extensions;
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
            var tableBuilder = new RelationalTableSettingsBuilder();
            tableBuilder.Add(new RelationalTableSettings(typeof(Person), "admin"));

            builder
                .SetPrimaryKeyColumnNamesFormat(x => x.ClrType.Name + "Id") // sets primary key column name to <EntityName>Id
                .SetDbColumnNamesFormat(x => x.ToLower()) // sets all property names to lowercase
                .SetDbTableNamesFormat(x => x.ToLower()) // sets all table names to lowercase
                .ApplyTableSettings(tableBuilder) // apply all of the settings from the tableBuilder variable above
                .UseIndexConvention();

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
