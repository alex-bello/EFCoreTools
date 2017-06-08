using System;
using System.Collections.Generic;
using System.Linq;
using EFCoreTools.Core.Attributes;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCoreTools.Core.Conventions
{
    /// <summary>
    /// Convention that creates indexes for data model properties with Index Attribute. Compound indexes are created 
    /// by decorating multiple properties with an Index Attribute that shares that same name. To make the compound 
    /// index unique, the Index Attribute for each property should have the IsUnique property set to true.
    /// </summary>
    public class IndexConvention : IConvention
    {
        /// <summary>
        /// Applies the convention to the provided ModelBuilder object and returns the object once the convention is applied.
        /// </summary>
        /// <returns>ModelBuilder</returns>
        public void Apply(ModelBuilder modelBuilder)
        {   
            // Iterate through entities in model 
            modelBuilder.Model.GetEntityTypes()?
            .Select(c => c.ClrType) // Get only the ClrTypes
            .ToList() // Create list to get ForEach iteration
            .ForEach(x => {
                var d = new List<IndexAttribute>(); // Will store all matched attributes for type
                x.GetProperties()
                    .Where(prop => prop.IsDefined(typeof(IndexAttribute), false)) // Get only properties that have IndexAttribute defined
                    .ToList() // Create list to get ForEach iteration
                    .ForEach(p => {
                        var index = p.GetCustomAttribute<IndexAttribute>(); // Get attribute
                        index.PropertyName = p.Name; // Set property name, used for grouping below
                        d.Add(index); // Add attribute to list

                        d?.GroupBy(attr => attr.Name).Select(sel => sel.Key).ToList()
                            .ForEach( i => {
                                var props = d.Where(n => n.Name == i).Select(a => a.PropertyName).ToArray();
                                var isUnique = (bool)d.FirstOrDefault(n => n.Name == i).IsUnique;
                                modelBuilder.Entity(x).HasIndex(props).IsUnique(isUnique);
                    });
                });
            });
        }
    }
}