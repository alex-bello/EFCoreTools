using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace EFCoreTools.Core.Attributes
{
    public static class AttributeExtensionMethods
    {
        public static List<PropertyInfo> GetPropertiesWithAttribute<T>(this ModelBuilder modelBuilder) 
            where T : Attribute
        {
            var d = new List<PropertyInfo>(); // Will store all matched attributes for type

            modelBuilder.Model.GetEntityTypes()?
            .Select(c => c.ClrType) // Get only the ClrTypes
            .ToList() // Create list to get ForEach iteration
            .ForEach(x => {
                d.AddRange(x.GetProperties()
                .Where(prop => prop.IsDefined(typeof(T), false)) // Get only properties that have IndexAttribute defined
                .ToList()); // Create list to get ForEach iteration
            });

            return d;
        }
    }
}