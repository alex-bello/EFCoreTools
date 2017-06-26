using EFCoreTools.Tests.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EFCoreTools.Tests
{
    [TestClass]
    public class ConventionTests
    {
        [TestMethod]
        public void GetAllTypePropertyAttributesTest()
        {
            // Assign
            var model = new ModelBuilder(new CoreConventionSetBuilder().CreateConventionSet());
            
            // Act
            //model.Entity<Person>().

            // Assert
        }
    }
}
