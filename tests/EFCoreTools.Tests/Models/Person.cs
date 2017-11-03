using EFCoreTools.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EFCoreTools.Tests.Models
{
    [TestClass]
    public class Person
    {
        /// <summary>
        /// Person's Id.
        /// </summary>
        [Key]
        [Column(Order = 0)]
        [StringLength(36)]
        [Index("Id", true, true)]
        public string Id { get; set; }
        
        /// <summary>
        /// Client's social security number without any formatting.
        /// </summary>
        [Column(Order = 1)]
        [StringLength(9)]
        [Index("SSN", false, true)]
        public string Ssn { get; set; }
        
         /// <summary>
        /// Person's last name.
        /// </summary>
        [Column(Order = 2)]
        [StringLength(100)]
        [Index("PersonFullName", 1)]
        public string LastName { get; set; }
        
        /// <summary>
        /// Person's first name.
        /// </summary>
        [Column(Order = 3)]
        [StringLength(100)]
        [Index("PersonFullName", 2)]
        public string FirstName { get; set; }
        
        /// <summary>
        /// Persons's middle name.
        /// </summary>
        [Column(Order = 4)]
        [StringLength(50)]
        [Index("PersonFullName", 3)]
        public string MiddleName { get; set; }
        
        /// <summary>
        /// Person's suffix.
        /// </summary>
        [Column(Order = 7)]
        [StringLength(50)]
        public string Suffix { get; set; }
        
        /// <summary>
        /// Person's Date of Birth.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        // Create simple Index for ZipCode property.
        [Index("ZipCode")]
        public string ZipCode { get; set; }
    }
}
