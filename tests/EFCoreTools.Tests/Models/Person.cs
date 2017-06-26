using EFCoreTools.Core.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EFCoreTools.Tests.Models
{
    [TestClass]
    public class Person
    {
        public string Id { get; set; }
        
        public string FirstName { get; set; }

        [Index("LastName")]
        public string LastName { get; set; }

        [Index("Identifiers")]
        public string SSN { get; set; }

        [Index("Identifiers")]
        public string ZipCode { get; set; }
    }
}
