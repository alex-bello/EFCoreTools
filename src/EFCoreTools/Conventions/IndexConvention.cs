using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;
using EFCoreTools.Extensions;
using EFCoreTools.Attributes;

namespace EFCoreTools.Conventions
{
    /// <summary>
    /// Convention that creates indexes for data model properties with Index Attribute. Compound indexes are created 
    /// by decorating multiple properties with an Index Attribute that shares that same name. To make the compound 
    /// index unique, the Index Attribute for each property should have the IsUnique property set to true.
    /// </summary>
    public class IndexConvention : IConvention
    {
        //TODO: 
        // - Implement Ordering of Properties in Index
        
        /// <summary>
        /// Applies the convention to the provided ModelBuilder object and returns the object once the convention is applied.
        /// </summary>
        /// <returns>ModelBuilder</returns>
        public void Apply(ModelBuilder modelBuilder)
        {   
            // Get IEnumerable<TypePropertyAttribute> of all properties decorated with IndexAttribute
            var typePropertyAttributes = modelBuilder.GetAllTypePropertyAttributes<IndexAttribute>();
            
            // Iterate through typePropertyAttributes and apply convention to all items in collection
            foreach (var t in typePropertyAttributes)
            {
                var pa = t.PropertyAttributes.GroupBy(a => a.Attribute.Name, b => b);

                // Loop through each grouping of IndexAttributes 
                foreach (var p in pa)
                {
                    modelBuilder.Entity(t.Type) // Get entity from Model
                    .HasIndex(p.Select(m => m.PropertyName).ToArray()) // Gets Index object for entity or creates new one if it doesn't already exist
                    .IsUnique(p.Select(m => m.Attribute).All(x => x.IsUnique == true)) // Sets IsUnique property to true for the Index object if all IndexAttribute.IsUnique properties are true.
                    .ForSqlServerIsClustered(p.Select(m => m.Attribute).All(x => x.IsClustered == true)); // Sets IsClustered property to true for the Index object if all IndexAttribute.IsClustered properties are true.
                }
            }
        }
    }
}