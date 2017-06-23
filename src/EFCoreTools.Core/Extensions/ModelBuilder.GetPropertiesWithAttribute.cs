using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace EFCoreTools.Core.Extensions
{
    public static class AttributeExtensionMethods
    {
        public static IEnumerable<Type> GetAllPropertiesWithAttribute<T>(this ModelBuilder modelBuilder) 
            where T : Attribute
        {
            return modelBuilder.Model.GetEntityTypes()
            .Select(i => i.ClrType)
            .Where(t => t.GetProperties().Where(p => p.IsDefined(typeof(T), false)).Any());
            //.SelectMany(t => t.GetProperties().Where(p => p.IsDefined(typeof(T), false)));
        }
    }
}