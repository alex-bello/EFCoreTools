using EFCoreTools.Tests.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace EFCoreTools.Tests
{
    [TestClass]
    public class ConventionTests
    {
        [TestMethod]
        public void IndexConventionTests()
        {
            // Assign
            var context = TestDbContext.New("index_convention_tests");

            // Act
            var personEntity = context.Model.FindEntityType(typeof(Person));
            var indexes = personEntity.GetIndexes();

            foreach (var index in indexes)
            {
                Console.WriteLine(string.Format("DeclaringEntityType: {0}", index.DeclaringEntityType.ClrType.Name));
                Console.WriteLine(string.Format("IsUnique: {0}", index.IsUnique));
                Console.WriteLine(string.Format("IndexName: {0}", index.Relational().Name));
                Console.WriteLine("");
            }

            // Assert
            Assert.IsNotNull(personEntity);
            Assert.AreEqual(indexes.Count(), 2);
            Assert.IsTrue(indexes.Any(x => x.Relational().Name == "IX_Person_LastName"));
            Assert.IsTrue(indexes.Any(x => x.Relational().Name == "IX_Person_SSN_ZipCode"));
        }
    }
}
