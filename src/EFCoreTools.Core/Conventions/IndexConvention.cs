using System;
using System.Collections.Generic;
using System.Linq;
using EFCoreTools.Core.Attributes;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;
using EFCoreTools.Core.Extensions;

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
            var typePropertyAttributes = modelBuilder.GetAllTypePropertyAttributes<IndexAttribute>();
            
            foreach (var t in typePropertyAttributes)
            {
                var pa = t.PropertyAttributes.GroupBy(a => a.Attribute.Name, b => b);

                foreach (var p in pa)
                {
                    modelBuilder.Entity(t.Type).HasIndex(p.Select(m => m.PropertyName).ToArray()).IsUnique(p.Select(m => m.Attribute).Any(x => x.IsUnique == true));
                }
            }
        }
    }
}