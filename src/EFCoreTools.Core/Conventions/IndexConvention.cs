using System;
using System.Collections.Generic;
using System.Linq;
using EFCoreTools.Core.Attributes;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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
            // Loop through the entities in the model
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Declare a list to store all IndexAttribute objects in current entity
                var d = new List<IndexAttribute>();
                
                // Loop through the current entity's properties
                foreach (var prop in entity.ClrType.GetProperties())
                {
                    // If IndexAttribute exists for current property, set PropertyName property and add to the list
                    prop.GetCustomAttributes<IndexAttribute>()?.ToList()?.ForEach(x => {
                        x.PropertyName = prop.Name;
                        d.Add(x);
                    });
                }

                // If list is not null, get list of distinct index names, loop through list and set index for property using Fluent API
                d?.GroupBy(x => x.Name).Select(sel => sel.Key).ToList()
                    .ForEach( ind => {
                        var props = d?.Where(n => n.Name == ind).Select(a => a.PropertyName).ToArray();
                        var isUnique = (bool)d.FirstOrDefault(n => n.Name == ind).IsUnique;
                        modelBuilder.Entity(entity.Name).HasIndex(props).IsUnique(isUnique);
                    });
            }
        }
    }
}