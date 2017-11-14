using EFCoreTools.Tests.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace EFCoreTools.Tests
{
    [TestClass]
    public class IndexTests
    {
        [TestMethod]
        public void CreateUniqueIndexTest()
        {
            /**** ASSIGN ****/

            // Create a TestDbContext object using SQLite db.
            var context = TestDbContext.New("index_convention_tests");

            /**** ACT ****/

            // Grab the Person Entity from the Model.
            var personEntity = context.Model.FindEntityType(typeof(Person));
            
            // Grab the indexes from the Person Entity in the model.
            var indexCount = personEntity.GetIndexes().Count();
            var idIndex = personEntity.GetIndexes().FirstOrDefault(x => x.Relational().Name.ToLower() == "ix_person_id");
            var ssnIndex = personEntity.GetIndexes().FirstOrDefault(x => x.Relational().Name.ToLower() == "ix_person_ssn");
            var zipCodeIndex = personEntity.GetIndexes().FirstOrDefault(x => x.Relational().Name.ToLower() == "ix_person_zipcode");
            var fullNameIndex = personEntity.GetIndexes().FirstOrDefault(x => x.Relational().Name.ToLower() == "ix_person_lastname_firstname_middlename");

            /**** ASSERT ****/

            // Verify that the Person Entity exists in the Model.
            Assert.IsNotNull(personEntity);
            
            // Verify that the Indexes defined in the Person Model were created in the Entity.
            Assert.AreEqual(indexCount, 4);

            // Verify Id Index Settings.
            Assert.IsNotNull(idIndex);
            Assert.IsTrue(idIndex.IsUnique);
            Assert.IsTrue(idIndex.SqlServer().IsClustered ?? false);

            // Verify SSN Index Settings.
            Assert.IsNotNull(ssnIndex);
            Assert.IsTrue(ssnIndex.IsUnique);
            Assert.IsFalse(ssnIndex.SqlServer().IsClustered ?? false);

            // Verify ZipCode Index Settings.
            Assert.IsNotNull(zipCodeIndex);
            Assert.IsFalse(zipCodeIndex.IsUnique);
            Assert.IsFalse(zipCodeIndex.SqlServer().IsClustered ?? false);

            // Verify FullName Index Settings.
            Assert.IsNotNull(fullNameIndex);
            Assert.IsFalse(fullNameIndex.IsUnique);
            Assert.IsFalse(fullNameIndex.SqlServer().IsClustered ?? false);
            Assert.AreEqual(fullNameIndex.Properties.Count(), 3);
        }
    }
}
