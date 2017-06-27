using System;
using System.Linq;
using EFCoreTools.Tests.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EFCoreTools.Tests
{
    [TestClass]
    public class EntityTableSettingsTests
    {
        [TestMethod]
        public void EntityTableSettingsSchemaTests()
        {
            // Assign
            var context = TestDbContext.New("entity_table_settings_schema_tests");

            // Act
            var personEntity = context.Model.FindEntityType(typeof(Person));

            Console.WriteLine(string.Format("EntityType: {0}", personEntity.ClrType.Name));
            Console.WriteLine(string.Format("SchemaName: {0}", personEntity.Relational().Schema));
            Console.WriteLine("");

            // Assert
            Assert.IsNotNull(personEntity);
            Assert.AreEqual(personEntity.Relational().Schema, "admin");
        }
    }
}