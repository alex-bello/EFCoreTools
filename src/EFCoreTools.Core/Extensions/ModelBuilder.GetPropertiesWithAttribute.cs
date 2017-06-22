using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace EFCoreTools.Core.Extensions
{
    public static class AttributeExtensionMethods
    {
        public static List<TypePropertiesWithAttributeList> GetPropertiesWithAttribute<T>(this ModelBuilder modelBuilder) 
            where T : Attribute
        {
            var d = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.ClrType.GetProperties().Where(p => p.IsDefined(typeof(T), false)), 
                (t, p) => new TypePropertiesWithAttributeList(t, p))
            .ToList();

            return d;
        }
    }
}